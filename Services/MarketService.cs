using Microsoft.EntityFrameworkCore;
using TradeSim.Data;
using TradeSim.Models.DTOs;
using TradeSim.Models.Market;
using TradeSim.Services.Market;
using TradeSim.Services.Providers;

namespace TradeSim.Services
{
    public class MarketService
    {
        private readonly TradeSimDbContext context;
        private readonly IMarketDataProvider marketDataProvider;
        private readonly SimulatedMarketEngine marketEngine;

        public MarketService(
            TradeSimDbContext context,
            IMarketDataProvider marketDataProvider,
            SimulatedMarketEngine marketEngine)
        {
            this.context = context;
            this.marketDataProvider = marketDataProvider;
            this.marketEngine = marketEngine;
        }

        public async Task<List<MarketQuoteDto>> GetMarketQuotesAsync(
            Guid userId)
        {
            // --------------------------------------------------
            // Get all tradable instruments
            // --------------------------------------------------

            var instruments =
                await context.TradableInstruments
                    .Include(i => i.MarketInstrumentProfile)
                    .Where(i => i.IsTradable)
                    .ToListAsync();

            // --------------------------------------------------
            // Initialize simulated market state
            // --------------------------------------------------

            foreach (var instrument in instruments)
            {
                var profile =
                    instrument.MarketInstrumentProfile;

                if (profile == null)
                    continue;

                var config =
                    new SimulatedMarketConfig
                    {
                        TradableInstrumentId =
                            instrument.Id,

                        Symbol =
                            instrument.Symbol,

                        BasePrice =
                            profile.BasePrice,

                        VolatilityPercent =
                            profile.VolatilityPercent,

                        Beta =
                            profile.Beta,

                        TrendBias =
                            profile.TrendBias,

                        MomentumBias =
                            profile.MomentumBias,

                        AverageVolume =
                            profile.AverageVolume,

                        PreviousClose =
                            profile.PreviousClose,

                        TickSize =
                            profile.TickSize,

                        PriceBandPercent =
                            profile.PriceBandPercent,

                        SupportLevel =
                            profile.SupportLevel,

                        ResistanceLevel =
                            profile.ResistanceLevel
                    };

                marketEngine.InitializeInstrument(config);
            }

            // --------------------------------------------------
            // Get user's favorite instruments
            // --------------------------------------------------

            var userWatchlistItems =
                await context.UserWatchlistItems
                .Where(x => x.UserId == userId)
                .Select(x => new
                {
                    x.TradableInstrumentId,
                    x.IsFavorite
                })
                .ToListAsync();

            var watchlistInstrumentIds =
                userWatchlistItems
                    .Select(x => x.TradableInstrumentId)
                    .ToHashSet();

            var favoriteInstrumentIds =
                userWatchlistItems
                    .Where(x => x.IsFavorite)
                    .Select(x => x.TradableInstrumentId)
                    .ToHashSet();

            // --------------------------------------------------
            // Generate market quotes
            // --------------------------------------------------

            var quotes =
                new List<MarketQuoteDto>();

            foreach (var instrument in instruments)
            {
                if (!marketEngine.HasState(instrument.Id))
                    continue;

                // Update simulated price
                marketEngine.UpdatePrice(
                    instrument.Id);

                var state =
                    marketEngine.GetState(
                        instrument.Id);

                quotes.Add(
                    new MarketQuoteDto
                    {
                        TradableInstrumentId =
                            instrument.Id,

                        Symbol =
                            state.Symbol,

                        Name =
                            instrument.Name,

                        LastPrice =
                            state.CurrentPrice,

                        Change =
                            state.Change,

                        ChangePercent =
                            state.ChangePercent,

                        Open =
                            state.Open,

                        High =
                            state.High,

                        Low =
                            state.Low,

                        Volume =
                            state.Volume,

                        // User-specific favorite status
                        IsInWatchlist = watchlistInstrumentIds.Contains(instrument.Id),

                        IsFavorite = favoriteInstrumentIds.Contains(instrument.Id)

                    });
            }

            return quotes;
        }

        public async Task<List<MarketQuoteDto>> SearchMarketQuotesAsync(
    Guid userId,
    string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return new List<MarketQuoteDto>();
            }

            searchText = searchText.Trim();

            // --------------------------------------------------
            // Find matching tradable instruments
            // --------------------------------------------------

            var instruments =
                await context.TradableInstruments
                    .Include(i => i.MarketInstrumentProfile)
                    .Where(i =>
                        i.IsTradable &&
                        (
                            EF.Functions.Like(i.Symbol, $"%{searchText}%") ||
                            EF.Functions.Like(i.Name, $"%{searchText}%")
                        ))
                    .Take(10)
                    .ToListAsync();

            // --------------------------------------------------
            // Get user's watchlist information
            // --------------------------------------------------

            var userWatchlistItems =
                await context.UserWatchlistItems
                    .Where(x => x.UserId == userId)
                    .Select(x => new
                    {
                        x.TradableInstrumentId,
                        x.IsFavorite
                    })
                    .ToListAsync();

            var watchlistInstrumentIds =
                userWatchlistItems
                    .Select(x => x.TradableInstrumentId)
                    .ToHashSet();

            var favoriteInstrumentIds =
                userWatchlistItems
                    .Where(x => x.IsFavorite)
                    .Select(x => x.TradableInstrumentId)
                    .ToHashSet();

            // --------------------------------------------------
            // Generate search results
            // --------------------------------------------------

            var results =
                new List<MarketQuoteDto>();

            foreach (var instrument in instruments)
            {
                var profile =
                    instrument.MarketInstrumentProfile;

                if (profile == null)
                    continue;

                // Initialize simulated market state
                if (!marketEngine.HasState(instrument.Id))
                {
                    var config =
                        new SimulatedMarketConfig
                        {
                            TradableInstrumentId =
                                instrument.Id,

                            Symbol =
                                instrument.Symbol,

                            BasePrice =
                                profile.BasePrice,

                            VolatilityPercent =
                                profile.VolatilityPercent,

                            Beta =
                                profile.Beta,

                            TrendBias =
                                profile.TrendBias,

                            MomentumBias =
                                profile.MomentumBias,

                            AverageVolume =
                                profile.AverageVolume,

                            PreviousClose =
                                profile.PreviousClose,

                            TickSize =
                                profile.TickSize,

                            PriceBandPercent =
                                profile.PriceBandPercent,

                            SupportLevel =
                                profile.SupportLevel,

                            ResistanceLevel =
                                profile.ResistanceLevel
                        };

                    marketEngine.InitializeInstrument(config);
                }

                // Update simulated price
                marketEngine.UpdatePrice(instrument.Id);

                var state =
                    marketEngine.GetState(instrument.Id);

                results.Add(
                    new MarketQuoteDto
                    {
                        TradableInstrumentId =
                            instrument.Id,

                        Symbol =
                            state.Symbol,

                        Name =
                            instrument.Name,

                        LastPrice =
                            state.CurrentPrice,

                        Change =
                            state.Change,

                        ChangePercent =
                            state.ChangePercent,

                        Open =
                            state.Open,

                        High =
                            state.High,

                        Low =
                            state.Low,

                        Volume =
                            state.Volume,

                        IsInWatchlist =
                            watchlistInstrumentIds.Contains(
                                instrument.Id),

                        IsFavorite =
                            favoriteInstrumentIds.Contains(
                                instrument.Id)
                    });
            }

            return results;
        }
        // --------------------------------------------------
        // Resolve the current, server-trusted price for a
        // single instrument. Used by OrderService so order
        // execution never relies on a client-supplied price.
        // --------------------------------------------------

        public async Task<decimal> GetCurrentPriceAsync(
            int tradableInstrumentId)
        {
            var instrument =
                await context.TradableInstruments
                    .Include(i => i.MarketInstrumentProfile)
                    .FirstOrDefaultAsync(
                        i => i.Id == tradableInstrumentId);

            if (instrument == null)
                throw new InvalidOperationException(
                    "Tradable instrument not found.");

            if (!instrument.IsTradable)
                throw new InvalidOperationException(
                    "This instrument is not tradable.");

            var profile = instrument.MarketInstrumentProfile;

            if (profile == null)
                throw new InvalidOperationException(
                    "Market profile not configured for this instrument.");

            if (!marketEngine.HasState(instrument.Id))
            {
                var config = new SimulatedMarketConfig
                {
                    TradableInstrumentId = instrument.Id,
                    Symbol = instrument.Symbol,
                    BasePrice = profile.BasePrice,
                    VolatilityPercent = profile.VolatilityPercent,
                    Beta = profile.Beta,
                    TrendBias = profile.TrendBias,
                    MomentumBias = profile.MomentumBias,
                    AverageVolume = profile.AverageVolume,
                    PreviousClose = profile.PreviousClose,
                    TickSize = profile.TickSize,
                    PriceBandPercent = profile.PriceBandPercent,
                    SupportLevel = profile.SupportLevel,
                    ResistanceLevel = profile.ResistanceLevel
                };

                marketEngine.InitializeInstrument(config);
            }

            marketEngine.UpdatePrice(instrument.Id);

            return marketEngine.GetState(instrument.Id).CurrentPrice;
        }
        public List<SimulatedMarketCandle> GetMarketCandles(int tradableInstrumentId)

        {
            return marketEngine.GetCandles(tradableInstrumentId);

        }
    }
}
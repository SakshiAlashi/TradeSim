//using Microsoft.EntityFrameworkCore;
//using TradeSim.Data;
//using TradeSim.Models.DTOs;
//using TradeSim.Models.Market;
//using TradeSim.Services.Market;
//using TradeSim.Services.Providers;

//namespace TradeSim.Services
//{
//    public class MarketService
//    {
//        private readonly TradeSimDbContext context;
//        private readonly IMarketDataProvider marketDataProvider;
//        private readonly SimulatedMarketEngine marketEngine;

//        public MarketService(
//            TradeSimDbContext context,
//            IMarketDataProvider marketDataProvider,
//            SimulatedMarketEngine marketEngine)
//        {
//            this.context = context;
//            this.marketDataProvider = marketDataProvider;
//            this.marketEngine = marketEngine;
//        }

//        public async Task<List<MarketQuoteDto>> GetMarketQuotesAsync(Guid userId)
//        {
//            // --------------------------------------------------
//            // Get all tradable instruments
//            // --------------------------------------------------

//            var instruments =
//                await context.TradableInstruments
//                    .Include(i => i.MarketInstrumentProfile)
//                    .Where(i => i.IsTradable)
//                    .ToListAsync();

//            // --------------------------------------------------
//            // Initialize simulated market state
//            // --------------------------------------------------

//            foreach (var instrument in instruments)
//            {
//                var profile = instrument.MarketInstrumentProfile;

//                if (profile == null)
//                    continue;

//                var config = new SimulatedMarketConfig
//                {
//                    TradableInstrumentId = instrument.Id,

//                    Symbol = instrument.Symbol,

//                    BasePrice = profile.BasePrice,

//                    VolatilityPercent =
//                        profile.VolatilityPercent,

//                    Beta = profile.Beta,

//                    TrendBias = profile.TrendBias,

//                    MomentumBias =
//                        profile.MomentumBias,

//                    AverageVolume =
//                        profile.AverageVolume,

//                    PreviousClose =
//                        profile.PreviousClose,

//                    TickSize =
//                        profile.TickSize,

//                    PriceBandPercent =
//                        profile.PriceBandPercent,

//                    SupportLevel =
//                        profile.SupportLevel,

//                    ResistanceLevel =
//                        profile.ResistanceLevel
//                };

//                marketEngine.InitializeInstrument(config);
//            }

//            // --------------------------------------------------
//            // Get user's favorite instruments
//            // --------------------------------------------------

//            var favoriteInstrumentIds =
//                await context.UserWatchlistItems
//                    .Where(x =>
//                        x.UserId == userId &&
//                        x.IsFavorite)
//                    .Select(x => x.TradableInstrumentId)
//                    .ToHashSetAsync();

//            // --------------------------------------------------
//            // Generate market quotes
//            // --------------------------------------------------

//            var quotes = new List<MarketQuoteDto>();

//            foreach (var instrument in instruments)
//            {
//                if (!marketEngine.HasState(instrument.Id))
//                    continue;

//                // Update simulated price
//                marketEngine.UpdatePrice(instrument.Id);

//                var state =
//                    marketEngine.GetState(instrument.Id);

//                quotes.Add(
//                    new MarketQuoteDto
//                    {
//                        Symbol = state.Symbol,

//                        Name = instrument.Name,

//                        LastPrice =
//                            state.CurrentPrice,

//                        Change =
//                            state.Change,

//                        ChangePercent =
//                            state.ChangePercent,

//                        Open =
//                            state.Open,

//                        High =
//                            state.High,

//                        Low =
//                            state.Low,

//                        Volume =
//                            state.Volume,

//                        // ------------------------------------------
//                        // User-specific favorite status
//                        // ------------------------------------------

//                        IsFavorite =
//                            favoriteInstrumentIds.Contains(
//                                instrument.Id)
//                    });
//            }

//            return quotes;
//        }
//    }
//}
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

            var favoriteInstrumentIds =
                await context.UserWatchlistItems
                    .Where(x =>
                        x.UserId == userId &&
                        x.IsFavorite)
                    .Select(x => x.TradableInstrumentId)
                    .ToHashSetAsync();

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
                        IsFavorite =
                            favoriteInstrumentIds.Contains(
                                instrument.Id)
                    });
            }

            return quotes;
        }
    }
}
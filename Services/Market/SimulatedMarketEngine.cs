using TradeSim.Models.Market;

namespace TradeSim.Services.Market
{
    public class SimulatedMarketEngine
    {
        private readonly Dictionary<int, SimulatedMarketState> marketStates = new();
        private readonly Dictionary<int, SimulatedMarketConfig> marketConfigs = new();
        private readonly Dictionary<int, List<SimulatedMarketCandle>> marketCandles = new();

        public SimulatedMarketState GetState(int tradableInstrumentId)
        {
            if (!marketStates.TryGetValue(
                    tradableInstrumentId,
                    out var state))
            {
                throw new InvalidOperationException(
                    "Market state not initialized for this instrument.");
            }

            return state;
        }

        public bool HasState(int tradableInstrumentId)
        {
            return marketStates.ContainsKey(tradableInstrumentId);
        }

        public List<SimulatedMarketCandle> GetCandles(int tradableInstrumentId)
        {
            if (!marketCandles.TryGetValue(
                    tradableInstrumentId,
                    out var candles))
            {
                return new List<SimulatedMarketCandle>();
            }

            return candles;
        }

        private void UpdateCandle(
    int tradableInstrumentId,
    SimulatedMarketState state)
        {
            // --------------------------------------------------
            // Get or create candle collection
            // --------------------------------------------------

            if (!marketCandles.TryGetValue(
                    tradableInstrumentId,
                    out var candles))
            {
                candles = new List<SimulatedMarketCandle>();

                marketCandles[tradableInstrumentId] = candles;
            }

            // --------------------------------------------------
            // Current candle time
            // --------------------------------------------------

            DateTime candleTime =
                new DateTime(
                    state.LastUpdated.Year,
                    state.LastUpdated.Month,
                    state.LastUpdated.Day,
                    state.LastUpdated.Hour,
                    state.LastUpdated.Minute,
                    0,
                    DateTimeKind.Utc);

            // --------------------------------------------------
            // Get current candle
            // --------------------------------------------------

            var currentCandle =
                candles.LastOrDefault();

            // --------------------------------------------------
            // Create first candle
            // --------------------------------------------------

            if (currentCandle == null ||
                currentCandle.Time != candleTime)
            {
                var newCandle =
                    new SimulatedMarketCandle
                    {
                        TradableInstrumentId =
                            tradableInstrumentId,

                        Symbol =
                            state.Symbol,

                        Time =
                            candleTime,

                        Open =
                            state.CurrentPrice,

                        High =
                            state.CurrentPrice,

                        Low =
                            state.CurrentPrice,

                        Close =
                            state.CurrentPrice,

                        Volume =
                            state.Volume
                    };

                candles.Add(newCandle);

                return;
            }

            // --------------------------------------------------
            // Update current candle
            // --------------------------------------------------

            currentCandle.High =
                Math.Max(
                    currentCandle.High,
                    state.CurrentPrice);

            currentCandle.Low =
                Math.Min(
                    currentCandle.Low,
                    state.CurrentPrice);

            currentCandle.Close =
                state.CurrentPrice;

            currentCandle.Volume =
                state.Volume;
        }

        public void InitializeInstrument(
            SimulatedMarketConfig config)
        {
            // --------------------------------------------------
            // Always make sure the configuration exists.
            // --------------------------------------------------

            marketConfigs[config.TradableInstrumentId] = config;

            // --------------------------------------------------
            // If the state already exists, don't recreate it.
            // --------------------------------------------------

            if (marketStates.ContainsKey(config.TradableInstrumentId))
                return;

            // --------------------------------------------------
            // Calculate circuit limits
            // --------------------------------------------------

            decimal previousClose =
                config.PreviousClose;

            decimal upperCircuit =
                Math.Round(
                    previousClose *
                    (1 + config.PriceBandPercent / 100m),
                    2);

            decimal lowerCircuit =
                Math.Round(
                    previousClose *
                    (1 - config.PriceBandPercent / 100m),
                    2);

            // --------------------------------------------------
            // Create initial market state
            // --------------------------------------------------

            marketStates[config.TradableInstrumentId] =
                new SimulatedMarketState
                {
                    TradableInstrumentId =
                        config.TradableInstrumentId,

                    Symbol =
                        config.Symbol,

                    PreviousClose =
                        previousClose,

                    Open =
                        previousClose,

                    CurrentPrice =
                        previousClose,

                    High =
                        previousClose,

                    Low =
                        previousClose,

                    Change = 0m,

                    ChangePercent = 0m,

                    Volume = 0,

                    UpperCircuit =
                        upperCircuit,

                    LowerCircuit =
                        lowerCircuit,

                    LastUpdated =
                        DateTime.UtcNow
                };
        }

        public void UpdatePrice(int tradableInstrumentId)
        {
            // --------------------------------------------------
            // Get market state
            // --------------------------------------------------

            var state =
                GetState(tradableInstrumentId);

            // --------------------------------------------------
            // Get market configuration
            // --------------------------------------------------

            if (!marketConfigs.TryGetValue(
                    tradableInstrumentId,
                    out var config))
            {
                throw new InvalidOperationException(
                    "Market configuration not initialized for this instrument.");
            }

            // --------------------------------------------------
            // Calculate small per-tick price movement
            //
            // VolatilityPercent represents DAILY volatility.
            // We therefore use only a small fraction of it
            // for each simulator update.
            // --------------------------------------------------

            decimal dailyVolatility =
                Math.Abs(config.VolatilityPercent);

            // Approximate number of active market seconds
            // represented by one trading session.
            const decimal tradingSecondsPerDay =
                23400m;

            decimal perTickVolatility =
                dailyVolatility /
                (decimal)Math.Sqrt(
                    (double)tradingSecondsPerDay);

            // Random shock between -1 and +1
            decimal randomShock =
                (decimal)(Random.Shared.NextDouble() * 2 - 1);

            // Calculate movement based on current price
            decimal movement =
                state.CurrentPrice *
                (perTickVolatility / 100m) *
                randomShock;

            // --------------------------------------------------
            // Calculate new price
            // --------------------------------------------------

            decimal newPrice =
                state.CurrentPrice + movement;

            // --------------------------------------------------
            // Respect circuit limits
            // --------------------------------------------------

            if (newPrice > state.UpperCircuit)
            {
                newPrice =
                    state.UpperCircuit;
            }

            if (newPrice < state.LowerCircuit)
            {
                newPrice =
                    state.LowerCircuit;
            }

            // --------------------------------------------------
            // Respect tick size
            // --------------------------------------------------

            if (config.TickSize > 0)
            {
                newPrice =
                    Math.Round(
                        newPrice / config.TickSize,
                        0,
                        MidpointRounding.AwayFromZero)
                    * config.TickSize;
            }

            // --------------------------------------------------
            // Final safety check after tick rounding
            // --------------------------------------------------

            if (newPrice > state.UpperCircuit)
            {
                newPrice =
                    state.UpperCircuit;
            }

            if (newPrice < state.LowerCircuit)
            {
                newPrice =
                    state.LowerCircuit;
            }

            // --------------------------------------------------
            // Update current price
            // --------------------------------------------------

            state.CurrentPrice =
                Math.Round(
                    newPrice,
                    2);

            // --------------------------------------------------
            // Update high
            // --------------------------------------------------

            if (state.CurrentPrice > state.High)
            {
                state.High =
                    state.CurrentPrice;
            }

            // --------------------------------------------------
            // Update low
            // --------------------------------------------------

            if (state.CurrentPrice < state.Low)
            {
                state.Low =
                    state.CurrentPrice;
            }

            // --------------------------------------------------
            // Update absolute change
            // --------------------------------------------------

            state.Change =
                Math.Round(
                    state.CurrentPrice -
                    state.PreviousClose,
                    2);

            // --------------------------------------------------
            // Update percentage change
            // --------------------------------------------------

            if (state.PreviousClose > 0)
            {
                state.ChangePercent =
                    Math.Round(
                        (state.Change /
                         state.PreviousClose) * 100m,
                        2);
            }

            // --------------------------------------------------
            // Simulate volume
            // --------------------------------------------------

            int volumeIncrease =
                Random.Shared.Next(
                    100,
                    5000);

            state.Volume +=
                volumeIncrease;

            // --------------------------------------------------
            // Update timestamp
            // --------------------------------------------------

            state.LastUpdated =
                DateTime.UtcNow;

            // --------------------------------------------------
            // Update current OHLC candle
            // --------------------------------------------------

            UpdateCandle(
                tradableInstrumentId,
                state);
        }
    }
}
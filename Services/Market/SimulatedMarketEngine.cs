using TradeSim.Models.Market;

namespace TradeSim.Services.Market
{
    public class SimulatedMarketEngine
    {
        private readonly Dictionary<int, SimulatedMarketState> marketStates = new();

        private readonly Dictionary<int, SimulatedMarketConfig> marketConfigs = new();

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

        public void UpdatePrice(
            int tradableInstrumentId)
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
            // Temporary price movement
            // --------------------------------------------------

            decimal movement =
                (decimal)(
                    Random.Shared.NextDouble() - 0.5
                ) * 10m;

            decimal newPrice =
                state.CurrentPrice + movement;

            // --------------------------------------------------
            // Respect circuit limits
            // --------------------------------------------------

            if (newPrice > state.UpperCircuit)
                newPrice = state.UpperCircuit;

            if (newPrice < state.LowerCircuit)
                newPrice = state.LowerCircuit;

            // --------------------------------------------------
            // Respect tick size
            // --------------------------------------------------

            if (config.TickSize > 0)
            {
                newPrice =
                    Math.Round(
                        newPrice / config.TickSize,
                        0) * config.TickSize;
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
        }
    }
}
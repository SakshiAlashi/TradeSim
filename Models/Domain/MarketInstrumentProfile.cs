namespace TradeSim.Models.Domain
{
    public class MarketInstrumentProfile
    {
        public int Id { get; set; }

        public int TradableInstrumentId { get; set; }

        // Starting/reference price for simulation
        public decimal BasePrice { get; set; }

        // Typical daily volatility percentage
        public decimal VolatilityPercent { get; set; }

        // How strongly the instrument reacts to overall market movement
        public decimal Beta { get; set; }

        // Long-term directional bias of the simulated instrument
        public decimal TrendBias { get; set; }

        // Short-term momentum tendency
        public decimal MomentumBias { get; set; }

        // Typical daily trading volume
        public long AverageVolume { get; set; }

        // Previous session reference
        public decimal PreviousClose { get; set; }

        // 52-week reference range for simulation
        public decimal FiftyTwoWeekHigh { get; set; }

        public decimal FiftyTwoWeekLow { get; set; }

        // Trading constraints
        public decimal TickSize { get; set; } = 0.05m;

        public decimal PriceBandPercent { get; set; } = 20m;

        // Reference technical levels
        public decimal SupportLevel { get; set; }

        public decimal ResistanceLevel { get; set; }

        // Navigation
        public TradableInstrument TradableInstrument { get; set; } = null!;
    }
}
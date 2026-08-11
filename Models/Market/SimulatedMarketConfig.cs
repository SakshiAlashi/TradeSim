namespace TradeSim.Models.Market
{
    public class SimulatedMarketConfig
    {
        public int TradableInstrumentId { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public decimal BasePrice { get; set; }

        public decimal VolatilityPercent { get; set; }

        public decimal Beta { get; set; }

        public decimal TrendBias { get; set; }

        public decimal MomentumBias { get; set; }

        public long AverageVolume { get; set; }

        public decimal PreviousClose { get; set; }

        public decimal TickSize { get; set; }

        public decimal PriceBandPercent { get; set; }

        public decimal SupportLevel { get; set; }

        public decimal ResistanceLevel { get; set; }
    }
}
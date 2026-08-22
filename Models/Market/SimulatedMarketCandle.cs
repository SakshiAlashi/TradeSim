namespace TradeSim.Models.Market
{
    public class SimulatedMarketCandle
    {
        public int TradableInstrumentId { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public DateTime Time { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public long Volume { get; set; }
    }
}
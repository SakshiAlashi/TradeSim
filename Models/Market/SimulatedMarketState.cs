namespace TradeSim.Models.Market
{
    public class SimulatedMarketState
    {
        public int TradableInstrumentId { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public decimal PreviousClose { get; set; }

        public decimal Open { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Change { get; set; }

        public decimal ChangePercent { get; set; }

        public long Volume { get; set; }

        public decimal UpperCircuit { get; set; }

        public decimal LowerCircuit { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
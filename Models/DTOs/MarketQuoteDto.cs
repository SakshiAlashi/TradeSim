namespace TradeSim.Models.DTOs
{
    public class MarketQuoteDto
    {
        public int TradableInstrumentId { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public decimal LastPrice { get; set; }

        public decimal Change { get; set; }

        public decimal ChangePercent { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public long Volume { get; set; }

        public bool IsFavorite { get; set; }
    }
}
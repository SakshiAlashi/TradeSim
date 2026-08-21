namespace TradeSim.Models.ViewModels
{
    public class StockDetailsViewModel
    {
        public string CompanyName { get; set; } = string.Empty;

        public string Symbol { get; set; } = string.Empty;

        public decimal LastPrice { get; set; }

        public decimal Change { get; set; }

        public decimal ChangePercent { get; set; }

        public int LotSize { get; set; }
        public int TradableInstrumentId { get; set; }
    }
}
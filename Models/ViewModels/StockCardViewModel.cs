namespace TradeSim.Models.ViewModels
{
    public class StockCardViewModel
    {
        public string Symbol { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal ChangePercent { get; set; }
    }
}
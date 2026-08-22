namespace TradeSim.Models.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string Exchange { get; set; } = string.Empty;

        public string Side { get; set; } = string.Empty;

        public string Product { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalAmount { get; set; }

        public string OrderType { get; set; } = string.Empty;

        public string Validity { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public decimal? ExecutedPrice { get; set; }

        public DateTime? ExecutedAt { get; set; }
    }
}
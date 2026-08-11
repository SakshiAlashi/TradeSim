namespace TradeSim.Models.Requests
{
    public class PlaceOrderRequest
    {
        public int TradableInstrumentId { get; set; }

        public string Side { get; set; } = string.Empty;

        public string Product { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public bool UsesCMP { get; set; }

        public string OrderType { get; set; } = string.Empty;

        public decimal? TriggerPrice { get; set; }

        public decimal? LimitPrice { get; set; }

        public string Validity { get; set; } = string.Empty;
    }
}
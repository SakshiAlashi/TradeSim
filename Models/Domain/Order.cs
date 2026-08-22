namespace TradeSim.Models.Domain
{
    public class Order
    {
        public int Id { get; set; }

        // Who placed the order?
        public Guid UserId { get; set; }

        // Which stock?
        public int TradableInstrumentId { get; set; }

        // BUY or SELL
        public string Side { get; set; } = string.Empty;

        // DELIVERY or INTRADAY
        public string Product { get; set; } = string.Empty;

        // Number of shares
        public int Quantity { get; set; }

        // Order price
        public decimal Price { get; set; }

        // Quantity × Price
        public decimal TotalAmount { get; set; }

        // Whether CMP was selected
        public bool UsesCMP { get; set; }

        // REGULAR / SL / SL-M / AMO / GTT
        public string OrderType { get; set; } = string.Empty;

        // Used by SL / SL-M / GTT
        public decimal? TriggerPrice { get; set; }

        // Used by SL / GTT
        public decimal? LimitPrice { get; set; }

        // DAY / IOC
        public string Validity { get; set; } = string.Empty;

        // OPEN / EXECUTED / CANCELLED / REJECTED
        public string Status { get; set; } = string.Empty;

        // Actual simulated execution price
        public decimal? ExecutedPrice { get; set; }

        public DateTime? ExecutedAt { get; set; }

        // When the user cancelled the order
        public DateTime? CancelledAt { get; set; }

        // Why the order was rejected
        public string? RejectionReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; } = null!;

        public TradableInstrument TradableInstrument { get; set; } = null!;
    }
}


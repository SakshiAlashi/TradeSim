namespace TradeSim.Models.Domain
{
    public class PortfolioHolding
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public int TradableInstrumentId { get; set; }

        public int Quantity { get; set; }

        public decimal AverageBuyPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties

        public User User { get; set; } = null!;

        public TradableInstrument TradableInstrument { get; set; } = null!;
    }
}
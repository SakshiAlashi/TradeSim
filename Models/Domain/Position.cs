using System;

namespace TradeSim.Models.Domain
{
    public class Position
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public int TradableInstrumentId { get; set; }

        public string Side { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal EntryPrice { get; set; }

        public string Product { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation properties

        public User User { get; set; } = null!;

        public TradableInstrument TradableInstrument { get; set; } = null!;
    }
}
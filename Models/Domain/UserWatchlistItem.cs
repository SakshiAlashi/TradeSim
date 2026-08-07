namespace TradeSim.Models.Domain
{
    public class UserWatchlistItem
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public int TradableInstrumentId { get; set; }

        public TradableInstrument TradableInstrument { get; set; } = null!;

        public bool IsFavorite { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
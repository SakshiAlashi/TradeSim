using Microsoft.EntityFrameworkCore;
using TradeSim.Data;
using TradeSim.Models.Domain;

namespace TradeSim.Services
{
    public class WatchlistService
    {
        private readonly TradeSimDbContext dbContext;

        public WatchlistService(TradeSimDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void ToggleFavorite(Guid userId, int instrumentId)
        {
            UserWatchlistItem? item = dbContext.UserWatchlistItems
                .FirstOrDefault(x =>
                    x.UserId == userId &&
                    x.TradableInstrumentId == instrumentId);

            if (item == null)
                return;

            item.IsFavorite = !item.IsFavorite;

            dbContext.SaveChanges();
        }
    }
}
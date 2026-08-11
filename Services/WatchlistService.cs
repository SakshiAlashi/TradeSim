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

        public async Task<List<UserWatchlistItem>> GetUserWatchlistAsync(
            Guid userId)
        {
            return await dbContext.UserWatchlistItems
                .Include(x => x.TradableInstrument)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task AddToWatchlistAsync(
            Guid userId,
            int instrumentId)
        {
            bool alreadyExists =
                await dbContext.UserWatchlistItems
                    .AnyAsync(x =>
                        x.UserId == userId &&
                        x.TradableInstrumentId == instrumentId);

            if (alreadyExists)
                return;

            var item = new UserWatchlistItem
            {
                UserId = userId,
                TradableInstrumentId = instrumentId,
                IsFavorite = false
            };

            dbContext.UserWatchlistItems.Add(item);

            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveFromWatchlistAsync(
            Guid userId,
            int instrumentId)
        {
            var item =
                await dbContext.UserWatchlistItems
                    .FirstOrDefaultAsync(x =>
                        x.UserId == userId &&
                        x.TradableInstrumentId == instrumentId);

            if (item == null)
                return;

            dbContext.UserWatchlistItems.Remove(item);

            await dbContext.SaveChangesAsync();
        }

        public async Task ToggleFavoriteAsync(
            Guid userId,
            int instrumentId)
        {
            var item =
                await dbContext.UserWatchlistItems
                    .FirstOrDefaultAsync(x =>
                        x.UserId == userId &&
                        x.TradableInstrumentId == instrumentId);

            if (item == null)
                return;

            item.IsFavorite = !item.IsFavorite;

            await dbContext.SaveChangesAsync();
        }
    }
}
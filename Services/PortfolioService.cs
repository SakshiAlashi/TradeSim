using Microsoft.EntityFrameworkCore;
using TradeSim.Data;
using TradeSim.Models.Domain;

namespace TradeSim.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly TradeSimDbContext _context;

        public PortfolioService(TradeSimDbContext context)
        {
            _context = context;
        }

        public async Task<List<PortfolioHolding>> GetHoldingsAsync(Guid userId)
        {
            return await _context.PortfolioHoldings
                .Where(h => h.UserId == userId)
                .Include(h => h.TradableInstrument)
                .ToListAsync();
        }

        public async Task<List<Position>> GetPositionsAsync(Guid userId)
        {
            return await _context.Positions
                .Where(p => p.UserId == userId)
                .Include(p => p.TradableInstrument)
                .ToListAsync();
        }
    }
}
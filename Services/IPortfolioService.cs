using TradeSim.Models.Domain;

namespace TradeSim.Services
{
    public interface IPortfolioService
    {
        Task<List<PortfolioHolding>> GetHoldingsAsync(Guid userId);

        Task<List<Position>> GetPositionsAsync(Guid userId);
    }
}
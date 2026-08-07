using TradeSim.Models.DTOs;
using TradeSim.Services.Providers;

namespace TradeSim.Services
{
    public class MarketService
    {
        private readonly IMarketDataProvider marketDataProvider;

        public MarketService(IMarketDataProvider marketDataProvider)
        {
            this.marketDataProvider = marketDataProvider;
        }

        public async Task<List<MarketQuoteDto>> GetMarketQuotesAsync()
        {
            return await marketDataProvider.GetMarketQuotesAsync();
        }
    }
}
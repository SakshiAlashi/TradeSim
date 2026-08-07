using TradeSim.Models.DTOs;

namespace TradeSim.Services.Providers
{
    public interface IMarketDataProvider
    {
        Task<List<MarketQuoteDto>> GetMarketQuotesAsync();

        Task<InstrumentDetailsDto> GetInstrumentDetailsAsync(string symbol);
    }
}
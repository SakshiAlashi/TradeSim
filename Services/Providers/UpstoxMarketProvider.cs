using TradeSim.Models.DTOs;

namespace TradeSim.Services.Providers
{
    public class UpstoxMarketProvider : IMarketDataProvider
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public UpstoxMarketProvider(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public async Task<List<MarketQuoteDto>> GetMarketQuotesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<InstrumentDetailsDto> GetInstrumentDetailsAsync(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
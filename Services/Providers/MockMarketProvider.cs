using TradeSim.Models.DTOs;

namespace TradeSim.Services.Providers
{
    public class MockMarketProvider : IMarketDataProvider
    {
        public async Task<List<MarketQuoteDto>> GetMarketQuotesAsync()
        {
            var quotes = new List<MarketQuoteDto>
    {
        new MarketQuoteDto
        {
            Symbol = "RELIANCE",
            Name = "Reliance Industries",
            LastPrice = 2845.50m,
            Change = 25.40m,
            ChangePercent = 0.90m,
            Open = 2820.10m,
            High = 2852.70m,
            Low = 2815.60m,
            Volume = 2587421
        },

        new MarketQuoteDto
        {
            Symbol = "TCS",
            Name = "Tata Consultancy Services",
            LastPrice = 4312.20m,
            Change = -12.50m,
            ChangePercent = -0.29m,
            Open = 4328.00m,
            High = 4335.40m,
            Low = 4302.15m,
            Volume = 985214
        },

        new MarketQuoteDto
        {
            Symbol = "INFY",
            Name = "Infosys",
            LastPrice = 1782.40m,
            Change = 18.70m,
            ChangePercent = 1.06m,
            Open = 1764.00m,
            High = 1788.20m,
            Low = 1759.30m,
            Volume = 1785324
        },

        new MarketQuoteDto
        {
            Symbol = "NIFTY",
            Name = "NIFTY 50",
            LastPrice = 25240.15m,
            Change = 142.30m,
            ChangePercent = 0.57m,
            Open = 25110.60m,
            High = 25255.40m,
            Low = 25082.20m,
            Volume = 0
        },

        new MarketQuoteDto
        {
            Symbol = "BANKNIFTY",
            Name = "NIFTY BANK",
            LastPrice = 56180.80m,
            Change = -95.60m,
            ChangePercent = -0.17m,
            Open = 56290.40m,
            High = 56410.25m,
            Low = 56095.30m,
            Volume = 0
        }
    };

            return await Task.FromResult(quotes);
        }

        public async Task<InstrumentDetailsDto> GetInstrumentDetailsAsync(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
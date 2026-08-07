using TradeSim.Models.DTOs;

namespace TradeSim.ViewModels
{
    public class WatchlistViewModel
    {
        // Top Cards
        public List<MarketQuoteDto> Indices { get; set; } = new();

        // Searchable Stock List
        public List<MarketQuoteDto> Stocks { get; set; } = new();

        // Right Panel
        public MarketQuoteDto? SelectedStock { get; set; }

        public string SearchText { get; set; } = string.Empty;

        public string SelectedInstrument { get; set; } = string.Empty;
    }
}
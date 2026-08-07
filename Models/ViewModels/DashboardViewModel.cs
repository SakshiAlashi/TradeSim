namespace TradeSim.Models.ViewModels
{
    public class DashboardViewModel
    {
        // Greeting
        public string Greeting { get; set; } = "";

        // Header
        public bool IsMarketOpen { get; set; }

        // KPI Cards
        public decimal VirtualBalance { get; set; }

        public decimal PortfolioValue { get; set; }

        public decimal TotalPnL { get; set; }

        public decimal TodayPnL { get; set; }

        public decimal PortfolioGrowthPercent { get; set; }

        public decimal TodayGrowthPercent { get; set; }

        // Dashboard Lists
        public List<StockCardViewModel> TopGainers { get; set; } = new();

        public List<StockCardViewModel> TopLosers { get; set; } = new();

        public List<IndexCardViewModel> MarketIndices { get; set; } = new();
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeSim.Services;
using TradeSim.ViewModels;
using System.Linq;

namespace TradeSim.Controllers
{
    [Authorize]
    public class WatchlistController : Controller
    {
        private readonly MarketService marketService;

        public WatchlistController(MarketService marketService)
        {
            this.marketService = marketService;
        }

        public async Task<IActionResult> Index()
        {
            var quotes = await marketService.GetMarketQuotesAsync();

            var viewModel = new WatchlistViewModel
            {
                Stocks = quotes,
                Indices = quotes.Where(x =>
                    x.Symbol == "NIFTY" ||
                    x.Symbol == "BANKNIFTY").ToList()
            };

            return View(viewModel);
        }
    }
}
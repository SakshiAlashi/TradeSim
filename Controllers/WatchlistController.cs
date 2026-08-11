using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeSim.Services;
using TradeSim.ViewModels;

namespace TradeSim.Controllers
{
    [Authorize]
    public class WatchlistController : Controller
    {
        private readonly MarketService marketService;

        public WatchlistController(
            MarketService marketService)
        {
            this.marketService = marketService;
        }

        public async Task<IActionResult> Index()
        {
            // --------------------------------------------------
            // Get logged-in user's ID
            // --------------------------------------------------

            var userIdClaim =
                User.FindFirst("UserId");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            if (!Guid.TryParse(
                    userIdClaim.Value,
                    out Guid userId))
            {
                return Unauthorized();
            }

            // --------------------------------------------------
            // Get ALL market quotes
            //
            // MarketService also determines whether each
            // instrument is a favorite for this user.
            // --------------------------------------------------

            var quotes =
                await marketService.GetMarketQuotesAsync(userId);

            // --------------------------------------------------
            // Build Watchlist ViewModel
            // --------------------------------------------------

            var viewModel = new WatchlistViewModel
            {
                // ALL available stocks are displayed.
                Stocks = quotes,

                // Indices are kept separately.
                Indices = quotes
                    .Where(x =>
                        x.Symbol == "NIFTY" ||
                        x.Symbol == "BANKNIFTY")
                    .ToList()
            };

            return View(viewModel);
        }
    }
}
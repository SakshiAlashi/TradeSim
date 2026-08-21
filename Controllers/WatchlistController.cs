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
        private readonly WatchlistService watchlistService;

        public WatchlistController(
            MarketService marketService,
            WatchlistService watchlistService)
        {
            this.marketService = marketService;
            this.watchlistService = watchlistService;
        }

        public async Task<IActionResult> Index()
        {
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

            var quotes =
                await marketService.GetMarketQuotesAsync(userId);

            var viewModel = new WatchlistViewModel
            {
                Stocks = quotes,

                Indices = quotes
                    .Where(x =>
                        x.Symbol == "NIFTY" ||
                        x.Symbol == "BANKNIFTY")
                    .ToList()
            };

            return View(viewModel);
        }


        // ==========================================================
        // ADD TO WATCHLIST
        // ==========================================================

        [HttpPost]
        public async Task<IActionResult> Add(int instrumentId)
        {
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

            await watchlistService.AddToWatchlistAsync(
                userId,
                instrumentId);

            return Ok(new
            {
                success = true
            });
        }

        // ==========================================================
        // REMOVE FROM WATCHLIST
        // ==========================================================

        [HttpPost]
        public async Task<IActionResult> Remove(int instrumentId)
        {
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

            await watchlistService.RemoveFromWatchlistAsync(
                userId,
                instrumentId);

            return Ok(new
            {
                success = true
            });
        }

        // ==========================================================
        // TOGGLE FAVORITE
        // ==========================================================

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int instrumentId)
        {
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

            var isFavorite =
                await watchlistService.ToggleFavoriteAsync(
                    userId,
                    instrumentId);

            return Ok(new
            {
                success = true,
                isFavorite = isFavorite
            });
        }
    }
}
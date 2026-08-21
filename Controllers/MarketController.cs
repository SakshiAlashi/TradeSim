using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeSim.Services;

namespace TradeSim.Controllers
{
    [Authorize]
    public class MarketController : Controller
    {
        private readonly MarketService marketService;

        public MarketController(
            MarketService marketService)
        {
            this.marketService = marketService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchText)
        {
            var userIdClaim = User.FindFirst("UserId");

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

            var results =
                await marketService.SearchMarketQuotesAsync(
                    userId,
                    searchText);

            return Json(results);
        }
    }
}
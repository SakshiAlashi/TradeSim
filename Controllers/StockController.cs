using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeSim.Services;
using TradeSim.Models.ViewModels;

namespace TradeSim.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        private readonly MarketService marketService;

        public StockController(
            MarketService marketService)
        {
            this.marketService = marketService;
        }

        public async Task<IActionResult> Details(string symbol)
        {
            // ==========================================================
            // GET CURRENT USER
            // ==========================================================

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


            // ==========================================================
            // GET MARKET QUOTES
            // ==========================================================

            var quotes =
                await marketService.GetMarketQuotesAsync(userId);


            // ==========================================================
            // FIND SELECTED STOCK
            // ==========================================================

            var stock =
                quotes.FirstOrDefault(x =>
                    x.Symbol.Equals(
                        symbol,
                        StringComparison.OrdinalIgnoreCase));


            if (stock == null)
            {
                return NotFound();
            }


            // ==========================================================
            // CREATE STOCK DETAILS VIEW MODEL
            // ==========================================================

            var vm = new StockDetailsViewModel
            {
                TradableInstrumentId = stock.TradableInstrumentId,

                CompanyName = stock.Name,
                Symbol = stock.Symbol,
                LastPrice = stock.LastPrice,
                Change = stock.Change,
                ChangePercent = stock.ChangePercent,

                // Temporary until we connect the real lot-size data
                LotSize = 1
            };


            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Candles(int tradableInstrumentId)
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

            // Make sure the instrument exists and
            // initialize/update its market state.
            await marketService.GetMarketQuotesAsync(userId);

            var candles =
                marketService.GetMarketCandles(
                    tradableInstrumentId);

            return Json(candles);
        }
    }
}
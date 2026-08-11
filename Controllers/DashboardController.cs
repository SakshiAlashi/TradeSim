using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeSim.Models.ViewModels;
using TradeSim.Services;

namespace TradeSim.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly MarketService marketService;

        public DashboardController(MarketService marketService)
        {
            this.marketService = marketService;
        }

        public async Task<IActionResult> Dashboard()
        {
            // Get the logged-in user's ID from the authentication cookie.
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized();
            }

            // Get current market quotes.
            //
            // The MarketService now requires the logged-in user's ID
            // so it can determine which stocks are favorites.
            var marketQuotes =
                await marketService.GetMarketQuotesAsync(userId);

            DashboardViewModel vm = new DashboardViewModel
            {
                Greeting = "Good Evening",

                IsMarketOpen = true,

                VirtualBalance = 100000,

                PortfolioValue = 100000,

                TotalPnL = 0,

                TodayPnL = 0,

                PortfolioGrowthPercent = 0,

                TodayGrowthPercent = 0,

                TopGainers = new List<StockCardViewModel>
                {
                    new()
                    {
                        Symbol = "RELIANCE",
                        Price = 2956.45m,
                        ChangePercent = 2.35m
                    },

                    new()
                    {
                        Symbol = "TCS",
                        Price = 4128.30m,
                        ChangePercent = 1.97m
                    },

                    new()
                    {
                        Symbol = "INFY",
                        Price = 1654.80m,
                        ChangePercent = 1.62m
                    }
                },

                TopLosers = new List<StockCardViewModel>
                {
                    new()
                    {
                        Symbol = "ITC",
                        Price = 423.20m,
                        ChangePercent = -1.25m
                    },

                    new()
                    {
                        Symbol = "HINDUNILVR",
                        Price = 2431.50m,
                        ChangePercent = -0.92m
                    },

                    new()
                    {
                        Symbol = "NESTLEIND",
                        Price = 2198.35m,
                        ChangePercent = -0.78m
                    }
                },

                MarketIndices = new List<IndexCardViewModel>
                {
                    new()
                    {
                        Name = "NIFTY 50",
                        Value = 24536.85m,
                        ChangePercent = 0.63m
                    },

                    new()
                    {
                        Name = "SENSEX",
                        Value = 81243.18m,
                        ChangePercent = 0.58m
                    },

                    new()
                    {
                        Name = "BANK NIFTY",
                        Value = 55326.35m,
                        ChangePercent = 0.71m
                    }
                }
            };

            return View(vm);
        }
    }
}
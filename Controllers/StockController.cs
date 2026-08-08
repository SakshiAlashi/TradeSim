using Microsoft.AspNetCore.Mvc;
using TradeSim.Models.ViewModels;

namespace TradeSim.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Details()
        {
            var vm = new StockDetailsViewModel
            {
                CompanyName = "Reliance Industries Ltd",
                Symbol = "RELIANCE",
                LastPrice = 2845.50m,
                Change = 25.40m,
                ChangePercent = 0.90m,
                LotSize = 100,
            };

            return View(vm);
        }
    }
}
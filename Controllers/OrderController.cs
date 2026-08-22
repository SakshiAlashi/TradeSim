using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TradeSim.Data;
using TradeSim.Models.Requests;
using TradeSim.Models.ViewModels;
using TradeSim.Services;

namespace TradeSim.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly TradeSimDbContext _context;

        public OrderController(IOrderService orderService, TradeSimDbContext context)
        {
            _orderService = orderService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(
            [FromBody] PlaceOrderRequest request)
        {
            try
            {
                // Get the logged-in user's ID from the authentication cookie
                var userIdClaim = User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Json(new
                    {
                        success = false,
                        message = "User is not logged in."
                    });
                }

                if (!Guid.TryParse(userIdClaim, out Guid userId))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid user information."
                    });
                }

                var order = await _orderService.PlaceOrderAsync(
                    userId,
                    request.TradableInstrumentId,
                    request.Side,
                    request.Product,
                    request.Quantity,
                    request.Price,
                    request.UsesCMP,
                    request.OrderType,
                    request.TriggerPrice,
                    request.LimitPrice,
                    request.Validity);

                return Json(new
                {
                    success = true,
                    message = "Order placed successfully.",
                    orderId = order.Id
                });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized();
            }

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.TradableInstrument)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    Symbol = o.TradableInstrument.Symbol,
                    CompanyName = o.TradableInstrument.Name,
                    Exchange = o.TradableInstrument.Exchange,

                    Side = o.Side,
                    Product = o.Product,
                    Quantity = o.Quantity,

                    Price = o.Price,
                    TotalAmount = o.TotalAmount,

                    OrderType = o.OrderType,
                    Validity = o.Validity,
                    Status = o.Status,

                    ExecutedPrice = o.ExecutedPrice,
                    ExecutedAt = o.ExecutedAt
                })
                .ToListAsync();

            return View(orders);
        }
        [HttpPost]
        public async Task<IActionResult> CancelOrder(
            [FromBody] CancelOrderRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Json(new
                    {
                        success = false,
                        message = "User is not logged in."
                    });
                }

                if (!Guid.TryParse(userIdClaim, out Guid userId))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid user information."
                    });
                }

                await _orderService.CancelOrderAsync(userId, request.OrderId);

                return Json(new
                {
                    success = true,
                    message = "Order cancelled successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
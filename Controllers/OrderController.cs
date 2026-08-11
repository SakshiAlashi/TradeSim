using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TradeSim.Models.Requests;
using TradeSim.Services;

namespace TradeSim.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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
    }
}
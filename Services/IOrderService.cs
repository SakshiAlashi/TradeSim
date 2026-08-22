//using TradeSim.Models.Domain;

//namespace TradeSim.Services
//{
//    public interface IOrderService
//    {
//        Task<Order> PlaceOrderAsync(
//            Guid userId,
//            int tradableInstrumentId,
//            string side,
//            string product,
//            int quantity,
//            decimal price,
//            bool usesCMP,
//            string orderType,
//            decimal? triggerPrice,
//            decimal? limitPrice,
//            string validity);
//    }
//}
using TradeSim.Models.Domain;

namespace TradeSim.Services
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(
            Guid userId,
            int tradableInstrumentId,
            string side,
            string product,
            int quantity,
            decimal price,
            bool usesCMP,
            string orderType,
            decimal? triggerPrice,
            decimal? limitPrice,
            string validity);

        Task CancelOrderAsync(Guid userId, int orderId);
        Task ExecuteOpenOrdersAsync();
    }
}
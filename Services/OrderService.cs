using Microsoft.EntityFrameworkCore;
using TradeSim.Data;
using TradeSim.Models.Domain;

namespace TradeSim.Services
{
    public class OrderService : IOrderService
    {
        private readonly TradeSimDbContext _context;
        private readonly MarketService _marketService;

        public OrderService(TradeSimDbContext context, MarketService marketService)
        {
            _context = context;
            _marketService = marketService;
        }

        public async Task<Order> PlaceOrderAsync(
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
            string validity)
        {
            // --------------------------------------------------
            // 1. Validate quantity
            // --------------------------------------------------

            if (quantity <= 0)
                throw new InvalidOperationException("Quantity must be greater than zero.");


            // --------------------------------------------------
            // 2. Validate side
            // --------------------------------------------------

            side = side.ToUpperInvariant();

            if (side != "BUY" && side != "SELL")
                throw new InvalidOperationException("Invalid order side.");


            // --------------------------------------------------
            // 3. Validate order type
            //
            // Only immediate-execution REGULAR orders are
            // supported right now — there's no resting-order /
            // trigger-watcher engine yet, so SL, SL-M, GTT, and
            // AMO orders must not be silently filled as if they
            // were regular market orders.
            // --------------------------------------------------

            var normalizedOrderType = orderType.ToUpperInvariant();

            if (normalizedOrderType != "REGULAR")
            {
                throw new InvalidOperationException(
                    $"Order type '{orderType}' is not supported yet. " +
                    "Only regular market/limit orders execute immediately " +
                    "in this simulator right now.");
            }


            // --------------------------------------------------
            // 4. Find user
            // --------------------------------------------------

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new InvalidOperationException("User not found.");


            // --------------------------------------------------
            // 5. Find instrument
            // --------------------------------------------------

            var instrument = await _context.TradableInstruments
                .FirstOrDefaultAsync(i => i.Id == tradableInstrumentId);

            if (instrument == null)
                throw new InvalidOperationException("Tradable instrument not found.");

            if (!instrument.IsTradable)
                throw new InvalidOperationException("This instrument is not tradable.");


            //// --------------------------------------------------
            //// 6. Resolve the real execution price server-side.
            ////
            //// The client-supplied `price` is NEVER trusted for
            //// money math — only `executionPrice`, resolved from
            //// the simulated market engine, is used below.
            //// --------------------------------------------------

            //decimal marketPrice =
            //    await _marketService.GetCurrentPriceAsync(tradableInstrumentId);

            //decimal executionPrice;

            //if (usesCMP)
            //{
            //    executionPrice = marketPrice;
            //}
            //else
            //{
            //    if (limitPrice is null or <= 0)
            //    {
            //        throw new InvalidOperationException(
            //            "A valid limit price is required when not using CMP.");
            //    }

            //    if (side == "BUY" && marketPrice > limitPrice.Value)
            //    {
            //        throw new InvalidOperationException(
            //            $"Order not marketable: current price ({marketPrice}) " +
            //            $"is above your limit price ({limitPrice.Value}).");
            //    }

            //    if (side == "SELL" && marketPrice < limitPrice.Value)
            //    {
            //        throw new InvalidOperationException(
            //            $"Order not marketable: current price ({marketPrice}) " +
            //            $"is below your limit price ({limitPrice.Value}).");
            //    }

            //    executionPrice = marketPrice;
            //}


            //// --------------------------------------------------
            //// 7. Calculate order value
            //// --------------------------------------------------

            //decimal totalAmount = quantity * executionPrice;

            //// --------------------------------------------------
            //// 7. BUY validation
            //// --------------------------------------------------

            //if (side == "BUY")
            //{
            //    // --------------------------------------------------
            //    // BUY validation
            //    // --------------------------------------------------

            //    if (user.Balance < totalAmount)
            //    {
            //        throw new InvalidOperationException(
            //            "Insufficient balance to place this order.");
            //    }

            //    // Deduct money from virtual balance
            //    user.Balance -= totalAmount;


            //    // --------------------------------------------------
            //    // DELIVERY → HOLDING
            //    // --------------------------------------------------

            //    if (product.ToUpperInvariant() == "DELIVERY")
            //    {
            //        var holding = await _context.PortfolioHoldings
            //            .FirstOrDefaultAsync(h =>
            //                h.UserId == userId &&
            //                h.TradableInstrumentId == tradableInstrumentId);

            //        if (holding == null)
            //        {
            //            // First DELIVERY purchase
            //            holding = new PortfolioHolding
            //            {
            //                UserId = userId,
            //                TradableInstrumentId = tradableInstrumentId,
            //                Quantity = quantity,
            //                AverageBuyPrice = executionPrice,
            //                CreatedAt = DateTime.UtcNow,
            //                UpdatedAt = DateTime.UtcNow
            //            };

            //            _context.PortfolioHoldings.Add(holding);
            //        }
            //        else
            //        {
            //            // Additional DELIVERY purchase
            //            decimal existingValue =
            //                holding.Quantity * holding.AverageBuyPrice;

            //            decimal newPurchaseValue =
            //                quantity * executionPrice;

            //            int newQuantity =
            //                holding.Quantity + quantity;

            //            holding.AverageBuyPrice =
            //                (existingValue + newPurchaseValue) / newQuantity;

            //            holding.Quantity = newQuantity;
            //            holding.UpdatedAt = DateTime.UtcNow;
            //        }
            //    }


            //    // --------------------------------------------------
            //    // INTRADAY → POSITION
            //    // --------------------------------------------------

            //    else if (product.ToUpperInvariant() == "INTRADAY")
            //    {
            //        var position = await _context.Positions
            //            .FirstOrDefaultAsync(p =>
            //                p.UserId == userId &&
            //                p.TradableInstrumentId == tradableInstrumentId &&
            //                p.Side == "BUY");

            //        if (position == null)
            //        {
            //            // First INTRADAY BUY
            //            position = new Position
            //            {
            //                UserId = userId,
            //                TradableInstrumentId = tradableInstrumentId,
            //                Side = "BUY",
            //                Quantity = quantity,
            //                EntryPrice = executionPrice,
            //                Product = "INTRADAY",
            //                CreatedAt = DateTime.UtcNow,
            //                UpdatedAt = DateTime.UtcNow
            //            };

            //            _context.Positions.Add(position);
            //        }
            //        else
            //        {
            //            // Additional INTRADAY BUY
            //            decimal existingValue =
            //                position.Quantity * position.EntryPrice;

            //            decimal newPurchaseValue =
            //                quantity * executionPrice;

            //            int newQuantity =
            //                position.Quantity + quantity;

            //            position.EntryPrice =
            //                (existingValue + newPurchaseValue) / newQuantity;

            //            position.Quantity = newQuantity;
            //            position.UpdatedAt = DateTime.UtcNow;
            //        }
            //    }
            //    else
            //    {
            //        throw new InvalidOperationException(
            //            "Invalid product type.");
            //    }
            //}


            //// --------------------------------------------------
            //// 8. SELL
            //// --------------------------------------------------

            //if (side == "SELL")
            //{
            //    // --------------------------------------------------
            //    // DELIVERY SELL
            //    // --------------------------------------------------

            //    if (product.ToUpperInvariant() == "DELIVERY")
            //    {
            //        var holding = await _context.PortfolioHoldings
            //            .FirstOrDefaultAsync(h =>
            //                h.UserId == userId &&
            //                h.TradableInstrumentId == tradableInstrumentId);

            //        if (holding == null)
            //        {
            //            throw new InvalidOperationException(
            //                "You do not own this instrument.");
            //        }

            //        if (quantity > holding.Quantity)
            //        {
            //            throw new InvalidOperationException(
            //                "You do not have enough quantity to sell.");
            //        }

            //        // Credit sale amount
            //        user.Balance += totalAmount;

            //        // Sell entire holding
            //        if (quantity == holding.Quantity)
            //        {
            //            _context.PortfolioHoldings.Remove(holding);
            //        }
            //        else
            //        {
            //            // Partial sale
            //            holding.Quantity -= quantity;
            //            holding.UpdatedAt = DateTime.UtcNow;
            //        }
            //    }


            //    // --------------------------------------------------
            //    // INTRADAY SELL
            //    // --------------------------------------------------

            //    else if (product.ToUpperInvariant() == "INTRADAY")
            //    {
            //        var position = await _context.Positions
            //            .FirstOrDefaultAsync(p =>
            //                p.UserId == userId &&
            //                p.TradableInstrumentId == tradableInstrumentId &&
            //                p.Side == "BUY" &&
            //                p.Product == "INTRADAY");

            //        if (position == null)
            //        {
            //            throw new InvalidOperationException(
            //                "You do not have an intraday position for this instrument.");
            //        }

            //        if (quantity > position.Quantity)
            //        {
            //            throw new InvalidOperationException(
            //                "You do not have enough quantity in your intraday position.");
            //        }

            //        // Credit sale amount
            //        user.Balance += totalAmount;

            //        // Close entire position
            //        if (quantity == position.Quantity)
            //        {
            //            _context.Positions.Remove(position);
            //        }
            //        else
            //        {
            //            // Partial exit
            //            position.Quantity -= quantity;
            //            position.UpdatedAt = DateTime.UtcNow;
            //        }
            //    }
            //}


            //// --------------------------------------------------
            //// 9. Create order
            //// --------------------------------------------------

            //var order = new Order
            //{
            //    UserId = userId,
            //    TradableInstrumentId = tradableInstrumentId,

            //    Side = side,
            //    Product = product.ToUpperInvariant(),

            //    Quantity = quantity,

            //    Price = executionPrice,
            //    TotalAmount = totalAmount,

            //    UsesCMP = usesCMP,

            //    OrderType = orderType.ToUpperInvariant(),

            //    TriggerPrice = triggerPrice,
            //    LimitPrice = limitPrice,

            //    Validity = validity.ToUpperInvariant(),

            //    Status = "EXECUTED",

            //    ExecutedPrice = executionPrice,
            //    ExecutedAt = DateTime.UtcNow,

            //    CreatedAt = DateTime.UtcNow
            //};

            // --------------------------------------------------
            // 6. Resolve market price and determine order state
            // --------------------------------------------------

            decimal marketPrice =
                await _marketService.GetCurrentPriceAsync(tradableInstrumentId);

            decimal orderPrice;
            bool shouldExecute;

            if (usesCMP)
            {
                // CMP orders execute immediately at current market price
                orderPrice = marketPrice;
                shouldExecute = true;
            }
            else
            {
                if (limitPrice is null || limitPrice <= 0)
                {
                    throw new InvalidOperationException(
                        "A valid limit price is required.");
                }

                orderPrice = limitPrice.Value;

                // BUY executes when CMP reaches or goes below target
                if (side == "BUY")
                {
                    shouldExecute = marketPrice <= orderPrice;
                }
                // SELL executes when CMP reaches or goes above target
                else
                {
                    shouldExecute = marketPrice >= orderPrice;
                }
            }


            // --------------------------------------------------
            // 7. Calculate order value
            // --------------------------------------------------

            decimal totalAmount = quantity * orderPrice;


            // --------------------------------------------------
            // 8. Validate whether the order can exist
            // --------------------------------------------------

            if (side == "BUY")
            {
                // For both OPEN and EXECUTED BUY orders,
                // the user must have enough balance available.
                if (user.Balance < totalAmount)
                {
                    throw new InvalidOperationException(
                        "Insufficient balance to place this order.");
                }
            }

            if (side == "SELL")
            {
                if (product.ToUpperInvariant() == "DELIVERY")
                {
                    var holding = await _context.PortfolioHoldings
                        .FirstOrDefaultAsync(h =>
                            h.UserId == userId &&
                            h.TradableInstrumentId == tradableInstrumentId);

                    if (holding == null)
                    {
                        throw new InvalidOperationException(
                            "You do not own this instrument.");
                    }

                    if (quantity > holding.Quantity)
                    {
                        throw new InvalidOperationException(
                            "You do not have enough quantity to sell.");
                    }
                }
                else if (product.ToUpperInvariant() == "INTRADAY")
                {
                    var position = await _context.Positions
                        .FirstOrDefaultAsync(p =>
                            p.UserId == userId &&
                            p.TradableInstrumentId == tradableInstrumentId &&
                            p.Side == "BUY" &&
                            p.Product == "INTRADAY");

                    if (position == null)
                    {
                        throw new InvalidOperationException(
                            "You do not have an intraday position for this instrument.");
                    }

                    if (quantity > position.Quantity)
                    {
                        throw new InvalidOperationException(
                            "You do not have enough quantity in your intraday position.");
                    }
                }
                else
                {
                    throw new InvalidOperationException(
                        "Invalid product type.");
                }
            }


            // --------------------------------------------------
            // 9. Create order
            // --------------------------------------------------

            var order = new Order
            {
                UserId = userId,
                TradableInstrumentId = tradableInstrumentId,

                Side = side,
                Product = product.ToUpperInvariant(),

                Quantity = quantity,

                // For OPEN orders this is the user's target price.
                // For EXECUTED orders this will be replaced by actual
                // execution price below.
                Price = orderPrice,

                TotalAmount = totalAmount,

                UsesCMP = usesCMP,

                OrderType = orderType.ToUpperInvariant(),

                TriggerPrice = triggerPrice,
                LimitPrice = limitPrice,

                Validity = validity.ToUpperInvariant(),

                Status = shouldExecute ? "EXECUTED" : "OPEN",

                ExecutedPrice = null,
                ExecutedAt = null,

                CreatedAt = DateTime.UtcNow
            };


            // --------------------------------------------------
            // 10. Execute immediately if target has been reached
            // --------------------------------------------------

            if (shouldExecute)
            {
                // The actual execution price is the current CMP.
                decimal executionPrice = marketPrice;

                decimal executionAmount =
                    quantity * executionPrice;

                order.Price = executionPrice;
                order.TotalAmount = executionAmount;

                order.ExecutedPrice = executionPrice;
                order.ExecutedAt = DateTime.UtcNow;


                // --------------------------------------------------
                // BUY EXECUTION
                // --------------------------------------------------

                if (side == "BUY")
                {
                    user.Balance -= executionAmount;


                    if (product.ToUpperInvariant() == "DELIVERY")
                    {
                        var holding = await _context.PortfolioHoldings
                            .FirstOrDefaultAsync(h =>
                                h.UserId == userId &&
                                h.TradableInstrumentId == tradableInstrumentId);

                        if (holding == null)
                        {
                            holding = new PortfolioHolding
                            {
                                UserId = userId,
                                TradableInstrumentId = tradableInstrumentId,
                                Quantity = quantity,
                                AverageBuyPrice = executionPrice,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };

                            _context.PortfolioHoldings.Add(holding);
                        }
                        else
                        {
                            decimal existingValue =
                                holding.Quantity * holding.AverageBuyPrice;

                            decimal newPurchaseValue =
                                quantity * executionPrice;

                            int newQuantity =
                                holding.Quantity + quantity;

                            holding.AverageBuyPrice =
                                (existingValue + newPurchaseValue) / newQuantity;

                            holding.Quantity = newQuantity;
                            holding.UpdatedAt = DateTime.UtcNow;
                        }
                    }
                    else if (product.ToUpperInvariant() == "INTRADAY")
                    {
                        var position = await _context.Positions
                            .FirstOrDefaultAsync(p =>
                                p.UserId == userId &&
                                p.TradableInstrumentId == tradableInstrumentId &&
                                p.Side == "BUY" &&
                                p.Product == "INTRADAY");

                        if (position == null)
                        {
                            position = new Position
                            {
                                UserId = userId,
                                TradableInstrumentId = tradableInstrumentId,
                                Side = "BUY",
                                Quantity = quantity,
                                EntryPrice = executionPrice,
                                Product = "INTRADAY",
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };

                            _context.Positions.Add(position);
                        }
                        else
                        {
                            decimal existingValue =
                                position.Quantity * position.EntryPrice;

                            decimal newPurchaseValue =
                                quantity * executionPrice;

                            int newQuantity =
                                position.Quantity + quantity;

                            position.EntryPrice =
                                (existingValue + newPurchaseValue) / newQuantity;

                            position.Quantity = newQuantity;
                            position.UpdatedAt = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            "Invalid product type.");
                    }
                }


                // --------------------------------------------------
                // SELL EXECUTION
                // --------------------------------------------------

                if (side == "SELL")
                {
                    if (product.ToUpperInvariant() == "DELIVERY")
                    {
                        var holding = await _context.PortfolioHoldings
                            .FirstOrDefaultAsync(h =>
                                h.UserId == userId &&
                                h.TradableInstrumentId == tradableInstrumentId);

                        user.Balance += executionAmount;

                        if (quantity == holding!.Quantity)
                        {
                            _context.PortfolioHoldings.Remove(holding);
                        }
                        else
                        {
                            holding.Quantity -= quantity;
                            holding.UpdatedAt = DateTime.UtcNow;
                        }
                    }
                    else if (product.ToUpperInvariant() == "INTRADAY")
                    {
                        var position = await _context.Positions
                            .FirstOrDefaultAsync(p =>
                                p.UserId == userId &&
                                p.TradableInstrumentId == tradableInstrumentId &&
                                p.Side == "BUY" &&
                                p.Product == "INTRADAY");

                        user.Balance += executionAmount;

                        if (quantity == position!.Quantity)
                        {
                            _context.Positions.Remove(position);
                        }
                        else
                        {
                            position.Quantity -= quantity;
                            position.UpdatedAt = DateTime.UtcNow;
                        }
                    }
                }
            }


            // --------------------------------------------------
            // 11. Save
            // --------------------------------------------------

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return order;


        }
        public async Task CancelOrderAsync(Guid userId, int orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o =>
                    o.Id == orderId &&
                    o.UserId == userId);

            if (order == null)
            {
                throw new InvalidOperationException(
                    "Order not found.");
            }

            if (order.Status != "OPEN")
            {
                throw new InvalidOperationException(
                    "Only open orders can be cancelled.");
            }

            order.Status = "CANCELLED";
            order.CancelledAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
        public async Task ExecuteOpenOrdersAsync()
        {
            var openOrders = await _context.Orders
                .Where(o => o.Status == "OPEN")
                .ToListAsync();

            foreach (var order in openOrders)
            {
                decimal marketPrice =
                    await _marketService.GetCurrentPriceAsync(
                        order.TradableInstrumentId);

                bool shouldExecute;

                if (order.Side == "BUY")
                {
                    shouldExecute =
                        order.LimitPrice.HasValue &&
                        marketPrice <= order.LimitPrice.Value;
                }
                else if (order.Side == "SELL")
                {
                    shouldExecute =
                        order.LimitPrice.HasValue &&
                        marketPrice >= order.LimitPrice.Value;
                }
                else
                {
                    continue;
                }
                if (!shouldExecute)
                {
                    continue;
                }

                order.Status = "EXECUTED";
                order.ExecutedPrice = marketPrice;
                order.ExecutedAt = DateTime.UtcNow;
                order.Price = marketPrice;
                order.TotalAmount = order.Quantity * marketPrice;

                if (order.Side == "BUY")
                {
                    var user = await _context.Users
                        .FirstOrDefaultAsync(u => u.Id == order.UserId);

                    if (user == null)
                    {
                        order.Status = "REJECTED";
                        order.RejectionReason = "User not found.";
                        continue;
                    }

                    decimal executionAmount =
                        order.Quantity * marketPrice;

                    if (user.Balance < executionAmount)
                    {
                        order.Status = "REJECTED";
                        order.RejectionReason =
                            "Insufficient balance at the time of execution.";

                        order.ExecutedPrice = null;
                        order.ExecutedAt = null;

                        continue;
                    }

                    user.Balance -= executionAmount;
                }
            }
        }
    }

}
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


            // --------------------------------------------------
            // 6. Resolve the real execution price server-side.
            //
            // The client-supplied `price` is NEVER trusted for
            // money math — only `executionPrice`, resolved from
            // the simulated market engine, is used below.
            // --------------------------------------------------

            decimal marketPrice =
                await _marketService.GetCurrentPriceAsync(tradableInstrumentId);

            decimal executionPrice;

            if (usesCMP)
            {
                executionPrice = marketPrice;
            }
            else
            {
                if (limitPrice is null or <= 0)
                {
                    throw new InvalidOperationException(
                        "A valid limit price is required when not using CMP.");
                }

                if (side == "BUY" && marketPrice > limitPrice.Value)
                {
                    throw new InvalidOperationException(
                        $"Order not marketable: current price ({marketPrice}) " +
                        $"is above your limit price ({limitPrice.Value}).");
                }

                if (side == "SELL" && marketPrice < limitPrice.Value)
                {
                    throw new InvalidOperationException(
                        $"Order not marketable: current price ({marketPrice}) " +
                        $"is below your limit price ({limitPrice.Value}).");
                }

                executionPrice = marketPrice;
            }


            // --------------------------------------------------
            // 7. Calculate order value
            // --------------------------------------------------

            decimal totalAmount = quantity * executionPrice;

            // --------------------------------------------------
            // 7. BUY validation
            // --------------------------------------------------

            if (side == "BUY")
            {
                // --------------------------------------------------
                // BUY validation
                // --------------------------------------------------

                if (user.Balance < totalAmount)
                {
                    throw new InvalidOperationException(
                        "Insufficient balance to place this order.");
                }

                // Deduct money from virtual balance
                user.Balance -= totalAmount;


                // --------------------------------------------------
                // DELIVERY → HOLDING
                // --------------------------------------------------

                if (product.ToUpperInvariant() == "DELIVERY")
                {
                    var holding = await _context.PortfolioHoldings
                        .FirstOrDefaultAsync(h =>
                            h.UserId == userId &&
                            h.TradableInstrumentId == tradableInstrumentId);

                    if (holding == null)
                    {
                        // First DELIVERY purchase
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
                        // Additional DELIVERY purchase
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


                // --------------------------------------------------
                // INTRADAY → POSITION
                // --------------------------------------------------

                else if (product.ToUpperInvariant() == "INTRADAY")
                {
                    var position = await _context.Positions
                        .FirstOrDefaultAsync(p =>
                            p.UserId == userId &&
                            p.TradableInstrumentId == tradableInstrumentId &&
                            p.Side == "BUY");

                    if (position == null)
                    {
                        // First INTRADAY BUY
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
                        // Additional INTRADAY BUY
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
            // 8. SELL
            // --------------------------------------------------

            if (side == "SELL")
            {
                // --------------------------------------------------
                // DELIVERY SELL
                // --------------------------------------------------

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

                    // Credit sale amount
                    user.Balance += totalAmount;

                    // Sell entire holding
                    if (quantity == holding.Quantity)
                    {
                        _context.PortfolioHoldings.Remove(holding);
                    }
                    else
                    {
                        // Partial sale
                        holding.Quantity -= quantity;
                        holding.UpdatedAt = DateTime.UtcNow;
                    }
                }


                // --------------------------------------------------
                // INTRADAY SELL
                // --------------------------------------------------

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

                    // Credit sale amount
                    user.Balance += totalAmount;

                    // Close entire position
                    if (quantity == position.Quantity)
                    {
                        _context.Positions.Remove(position);
                    }
                    else
                    {
                        // Partial exit
                        position.Quantity -= quantity;
                        position.UpdatedAt = DateTime.UtcNow;
                    }
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

                Price = executionPrice,
                TotalAmount = totalAmount,

                UsesCMP = usesCMP,

                OrderType = orderType.ToUpperInvariant(),

                TriggerPrice = triggerPrice,
                LimitPrice = limitPrice,

                Validity = validity.ToUpperInvariant(),

                Status = "EXECUTED",

                ExecutedPrice = executionPrice,
                ExecutedAt = DateTime.UtcNow,

                CreatedAt = DateTime.UtcNow
            };


            // --------------------------------------------------
            // 10. Save
            // --------------------------------------------------

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();


            return order;
        }
    }
}
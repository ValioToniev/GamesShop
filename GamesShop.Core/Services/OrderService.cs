using GamesShop.Core.Contracts;
using GamesShop.Infrastructure.Cart;
using GamesShop.Infrastructure.Data;
using GamesShop.Infrastructure.Data.Entities;
using GamesShop.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace GamesShop.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICartService _cartService;
        private readonly ApplicationDbContext _dbContext;

        public OrderService(ICartService cartService, ApplicationDbContext dbContext)
        {
            _cartService = cartService;
            _dbContext = dbContext;
        }
        public List<Order> GetOrdersByUser(string userId)
        {
            return _dbContext.Orders.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OrderDate)
                .ToList();
        }

        public List<Order> GetOrders()
        {
            return _dbContext.Orders.OrderByDescending(x => x.OrderDate).ToList();
        }

        public async Task<Order> CreateOrderAsync(string userId, List<CartModel> cartItems)
        {
            if (cartItems == null || !cartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty.");
            }

            Order order = new Order
            {
                OrderDate = DateTime.UtcNow,
                UserId = userId,
                Status = OrderStatus.Pending,
                OrderItems = new List<OrderItem>()
            };

            decimal totalOrderPrice = 0;

            foreach (var item in cartItems)
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);

                if (product == null || product.Quantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Not enough stock for {item.ProductName}. Available: {product?.Quantity ?? 0}");
                }

                decimal effectivePrice = item.CurrentPrice * (1 - item.CurrentDiscountPercentage / 100);

                OrderItem orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = effectivePrice
                };

                order.OrderItems.Add(orderItem);

                // 🔹 Reduce stock
                product.Quantity -= item.Quantity;
                _dbContext.Products.Update(product);

                totalOrderPrice += effectivePrice * item.Quantity;
            }

            order.TotalPrice = totalOrderPrice;

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            _cartService.ClearCart();
            return order;
        }
        public bool UpdateOrderStatus(int orderId, OrderStatus newStatus)
        {
            var order = _dbContext.Orders.Find(orderId);
            if (order == null) return false;

            order.Status = newStatus;
            _dbContext.Orders.Update(order);
            return _dbContext.SaveChanges() > 0;
        }
    }

}

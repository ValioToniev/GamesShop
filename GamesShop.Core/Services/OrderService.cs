using GamesShop.Core.Contracts;
using GamesShop.Infrastructure.Data;
using GamesShop.Infrastructure.Data.Entities;
using GamesShop.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Order> CreateOrderAsync(string userId)
        {
            // Retrieve the cart items from the cookie and database.
            var cartItems = _cartService.GetCartItems();
            if (!cartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty.");
            }

            // Create a new order.
            Order order = new Order
            {
                OrderDate = DateTime.UtcNow,
                UserId = userId,
                Status = Infrastructure.Enums.OrderStatus.Pending,
                OrderItems = new System.Collections.Generic.List<OrderItem>()
            };

            decimal totalOrderPrice = 0;

            // For each cart item, create an OrderItem.
            foreach (var item in cartItems)
            {
                // Calculate effective price (after discount).
                decimal effectivePrice = item.CurrentPrice * (1 - item.CurrentDiscountPercentage / 100);
                OrderItem orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = effectivePrice
                };

                order.OrderItems.Add(orderItem);
                totalOrderPrice += effectivePrice * item.Quantity;
            }

            order.TotalPrice = totalOrderPrice;

            // Save the order in the database.
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            // Clear the cart after successful order.
            _cartService.ClearCart();

            return order;
        }
    }

}

using GamesShop.Core.Contracts;
using GamesShop.Infrastructure.Data;
using GamesShop.Infrastructure.Data.Entities;
using GamesShop.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Create(int productId, string userId, int quantity)
        {
            // Find the product by its ID
            var product = _context.Products.SingleOrDefault(x => x.Id == productId);

            // Check if the product exists
            if (product == null || product.Quantity < quantity)
            {
                return false; // Return false if the product doesn't exist or not enough stock
            }

            // Create a new order
            Order item = new Order
            {
                OrderDate = DateTime.Now,
                ProductId = productId,
                UserId = userId,
                Quantity = quantity,
                CurrentPrice = product.Price, // Renamed to match Order table
                CurrentDiscountPercentage = product.Discount // Renamed to match Order table
            };

            // Reduce product stock
            product.Quantity -= quantity;

            // Save changes
            _context.Products.Update(product);
            _context.Orders.Add(item);
            return _context.SaveChanges() != 0;
        }



        public List<Order> GetOrders()
        {
            return _context.Orders.OrderByDescending(x => x.OrderDate).ToList();
        }

        public List<Order> GetOrdersByUser(string userId)
        {
            return _context.Orders.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OrderDate)
                .ToList();
        }
        public bool UpdateOrderStatus(int orderId, OrderStatus newStatus)
        {
            var order = _context.Orders.Find(orderId);
            if (order == null) return false;

            order.Status = newStatus;
            _context.Orders.Update(order);
            return _context.SaveChanges() > 0;
        }

    }

}

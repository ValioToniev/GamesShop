using GamesShop.Infrastructure.Cart;
using GamesShop.Infrastructure.Data.Entities;
using GamesShop.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Core.Contracts
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string userId, List<CartModel> cartItems);
        List<Order> GetOrders();
        List<Order> GetOrdersByUser(string userId);
        bool UpdateOrderStatus(int orderId, OrderStatus newStatus);
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesShop.Infrastructure.Cart;
using GamesShop.Infrastructure.Data.Entities;

namespace GamesShop.Core.Contracts
{
    public interface ICartService
    {
        List<CartModel> GetCartItems();
        Product GetProductById(int productId);
        void AddToCart(int productId, string productName, string picture, decimal price, decimal discount);
        void RemoveFromCart(int productId);
        void ClearCart();
    }

}

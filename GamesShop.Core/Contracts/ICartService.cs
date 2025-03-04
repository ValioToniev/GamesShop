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
        Dictionary<int, int> GetCart();

        void SaveCart(Dictionary<int, int> cart);

        void AddToCart(int productId);

        List<CartModel> GetCartItems();

        void ClearCart();
    }

}

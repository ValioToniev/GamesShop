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
        public List<CartModel> GetProductsByIds(List<int> productIds);
    }

}

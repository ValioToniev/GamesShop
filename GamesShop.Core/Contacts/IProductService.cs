using GamesShop.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Core.Contacts
{
    public interface IProductService
    {
        bool Create(string name, int genreId, int categoryId,string producer, string picture,string description, int quantity, decimal price, decimal discount);

        bool Update(int productId, string name, int genreId, int categoryId, string producer, string picture, string description, int quantity, decimal price, decimal discount);

        List<Product> GetProducts();

        Product GetProductById(int productId);

        bool RemoveById(int productId);

        List<Product> GetProducts(string searchStringCategoryName, string searchStringBrandName);
    }

}

using GamesShop.Core.Contracts;
using GamesShop.Infrastructure.Data.Entities;
using GamesShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Create(string name, int brandId, int categoryId,string producer, string picture,string description, int quantity, decimal price, decimal discount)
        {
            // Creating a new Product object
            Product item = new Product
            {
                ProductName = name,
                Genre = _context.Genres.Find(brandId),
                Category = _context.Categories.Find(categoryId),
                Producer = producer,
                Picture = picture,
                Description = description,
                Quantity = quantity,
                Price = price,
                Discount = discount
            };

            // Adding the product to the Products DbSet and saving changes to the database
            _context.Products.Add(item);
            return _context.SaveChanges() != 0;



        }
        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        public List<Product> GetProducts()
        {
            List<Product> products = _context.Products.ToList();
            return products;
        }
        public List<Product> GetProducts(string searchStringCategoryName, string searchStringGenreName)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchStringCategoryName) && !string.IsNullOrEmpty(searchStringGenreName))
            {
                products = products.Where(x => x.Category.CategoryName.ToLower().Contains(searchStringCategoryName.ToLower()) ||
                                                x.Genre.GenreName.ToLower().Contains(searchStringGenreName.ToLower()));
            }
            else if (!string.IsNullOrEmpty(searchStringCategoryName))
            {
                products = products.Where(x => x.Category.CategoryName.ToLower().Contains(searchStringCategoryName.ToLower()));
            }
            else if (!string.IsNullOrEmpty(searchStringGenreName))
            {
                products = products.Where(x => x.Genre.GenreName.ToLower().Contains(searchStringGenreName.ToLower()));
            }

            return products.ToList();
        }

        public bool RemoveById(int productId)
        {
            var product = GetProductById(productId);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            return _context.SaveChanges() != 0;
        }
        public bool Update(int productId, string name, int genreId, int categoryId, string producer, string picture, string description, int quantity, decimal price, decimal discount)
        {
            var product = GetProductById(productId);
            if (product == null) 
                return false;

            product.ProductName = name;
            product.GenreId = genreId; 
            product.CategoryId = categoryId; 
            product.Producer = producer;
            product.Picture = picture;
            product.Description = description;
            product.Quantity = quantity;
            product.Price = price;
            product.Discount = discount;

            _context.Update(product);
            return _context.SaveChanges() != 0;
        }


    }

}

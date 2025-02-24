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
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Category> GetCategories()
        {
            List<Category> categories = _context.Categories.ToList();
            return categories;
        }

        public Category GetCategoryById(int categoryId)
        {
            // Fetch a specific category by its ID
            return _context.Categories.Find(categoryId);
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            // Fetch all products associated with a specific category
            return _context.Products.Where(x => x.CategoryId == categoryId).ToList();
        }
    }

}

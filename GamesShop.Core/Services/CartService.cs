using GamesShop.Core.Contracts;
using GamesShop.Infrastructure.Cart;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GamesShop.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using GamesShop.Infrastructure.Data;

namespace GamesShop.Core.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CartModel> GetProductsByIds(List<int> productIds)
        {
            return _context.Products.Where(p => productIds.Contains(p.Id)).Select(x =>new CartModel
            {
                ProductId = x.Id,
                ProductName = x.ProductName,
                Picture = x.Picture,
                Quantity = x.Quantity,
                CurrentPrice = x.Price,
                CurrentDiscountPercentage = x.Discount,
                TotalPrice = x.Quantity * x.Price * (1 - x.Discount / 100)
            }).ToList();
        }
    }
}

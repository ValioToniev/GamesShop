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
        private const string CartSessionKey = "ShoppingCart";
        private readonly ApplicationDbContext _context;
        private readonly ISession _session;

        public CartService(IHttpContextAccessor httpContextAccessor,ApplicationDbContext context)
        {
            _session = httpContextAccessor.HttpContext!.Session;
            this._context = context;
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }

        public List<CartModel> GetCartItems()
        {
            var value = _session.GetString(CartSessionKey);
            return value == null ? new List<CartModel>() : JsonConvert.DeserializeObject<List<CartModel>>(value) ?? new List<CartModel>();
        }

        public void AddToCart(int productId, string productName, string picture, decimal price, decimal discount)
        {
            var cart = GetCartItems();
            var existingItem = cart.FirstOrDefault(p => p.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartModel
                {
                    ProductId = productId,
                    ProductName = productName,
                    Picture = picture,
                    CurrentPrice = price,
                    CurrentDiscountPercentage = discount,
                    Quantity = 1
                });
            }

            SaveCart(cart);
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCartItems();
            cart.RemoveAll(p => p.ProductId == productId);
            SaveCart(cart);
        }

        public void ClearCart()
        {
            _session.Remove(CartSessionKey);
        }

        private void SaveCart(List<CartModel> cart)
        {
            _session.SetString(CartSessionKey, JsonConvert.SerializeObject(cart));
        }
    }
}

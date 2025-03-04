using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using GamesShop.Infrastructure.Cart;
using GamesShop.Infrastructure.Data.Entities;
using GamesShop.Core.Contracts;
using GamesShop.Infrastructure.Data;

namespace GamesShop.Core.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext dbContext;
        private const string CartCookieKey = "cart";

        public CartService(IHttpContextAccessor httpContextAccessor, 
            ApplicationDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public Dictionary<int, int> GetCart()
        {
            var context = _httpContextAccessor.HttpContext;
            var cartCookie = context.Request.Cookies[CartCookieKey];
            if (!string.IsNullOrEmpty(cartCookie))
            {
                return JsonConvert.DeserializeObject<Dictionary<int, int>>(cartCookie)
                    ?? new Dictionary<int, int>();
            }
            return new Dictionary<int, int>();
        }

        public void SaveCart(Dictionary<int, int> cart)
        {
            var context = _httpContextAccessor.HttpContext;
            var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(7) };
            context.Response.Cookies.Append(CartCookieKey, JsonConvert.SerializeObject(cart), cookieOptions);
        }

        public void AddToCart(int productId)
        {
            var cart = GetCart();
            if (cart.ContainsKey(productId))
            {
                cart[productId]++;
            }
            else
            {
                cart[productId] = 1;
            }
            SaveCart(cart);
        }

        public List<CartModel> GetCartItems()
        {
            var cart = GetCart();
            var productIds = cart.Keys.ToList();

            var products = this.dbContext.Products.Where(p => productIds.Contains(p.Id)).ToList();

            var cartItems = products.Select(product =>
            {
                int quantity = cart[product.Id];
                decimal currentPrice = product.Price;
                decimal discountPercentage = product.Discount;
                decimal totalPrice = currentPrice * quantity * (1 - discountPercentage / 100);

                return new CartModel
                {
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    Picture = product.Picture,
                    Quantity = quantity,
                    CurrentPrice = currentPrice,
                    CurrentDiscountPercentage = discountPercentage,
                    TotalPrice = totalPrice
                };
            }).ToList();

            return cartItems;
        }

        // New method to clear the cart cookie.
        public void ClearCart()
        {
            var context = _httpContextAccessor.HttpContext;
            context.Response.Cookies.Delete(CartCookieKey);
        }
    }

}

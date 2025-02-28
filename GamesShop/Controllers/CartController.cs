using GamesShop.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GamesShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            var productIds = GetCartProductIdsFromCookies();
            var cartItems = _cartService.GetProductsByIds(productIds);
            return View(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var productIds = GetCartProductIdsFromCookies();
            if (!productIds.Contains(productId))
            {
                productIds.Add(productId);
                SaveCartProductIdsToCookies(productIds);
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var productIds = GetCartProductIdsFromCookies();
            productIds.Remove(productId);
            SaveCartProductIdsToCookies(productIds);
            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            Response.Cookies.Delete("ShoppingCart");
            return RedirectToAction("Index");
        }

        private List<int> GetCartProductIdsFromCookies()
        {
            var cookieValue = Request.Cookies["ShoppingCart"];
            return string.IsNullOrEmpty(cookieValue)
                ? new List<int>()
                : JsonConvert.DeserializeObject<List<int>>(cookieValue) ?? new List<int>();
        }

        private void SaveCartProductIdsToCookies(List<int> productIds)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(3),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("ShoppingCart", JsonConvert.SerializeObject(productIds), options);
        }
    }
}
using GamesShop.Core.Contracts;
using GamesShop.Infrastructure.Cart;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using GamesShop.Infrastructure.Data;
using GamesShop.Infrastructure.Data.Entities;

namespace GamesShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ApplicationDbContext _context;

        public CartController(ICartService cartService, ApplicationDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        public IActionResult Index()
        {
            var cartItems = GetCartItemsFromCookies();
            return View(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var cartItems = GetCartItemsFromCookies();

            var existingItem = cartItems.FirstOrDefault(p => p.ProductId == productId);
            if (existingItem == null)
            {
                var product = _cartService.GetProductById(productId);
                if (product == null) return NotFound();

                cartItems.Add(new CartModel
                {
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    Picture = product.Picture,
                    Quantity = 1, // User can update later
                    CurrentPrice = product.Price,
                    CurrentDiscountPercentage = product.Discount
                });
            }
            else
            {
                existingItem.Quantity += 1; // If in cart, increase quantity
            }

            SaveCartItemsToCookies(cartItems);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCart(Dictionary<int, int> quantities)
        {
            var cartItems = GetCartItemsFromCookies();

            foreach (var item in cartItems)
            {
                if (quantities.ContainsKey(item.ProductId))
                {
                    var product = _cartService.GetProductById(item.ProductId);
                    if (product == null) continue;

                    // Ensure the requested quantity does not exceed stock
                    int requestedQuantity = Math.Min(quantities[item.ProductId], product.Quantity);
                    item.Quantity = requestedQuantity;
                }
            }

            SaveCartItemsToCookies(cartItems);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cartItems = GetCartItemsFromCookies();
            cartItems.RemoveAll(p => p.ProductId == productId);
            SaveCartItemsToCookies(cartItems);
            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            Response.Cookies.Delete("ShoppingCart");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PlaceOrder(Dictionary<int, int> quantities)
        {
            var cartItems = GetCartItemsFromCookies();
            if (!cartItems.Any()) return RedirectToAction("Index");

            foreach (var cartItem in cartItems)
            {
                var product = _cartService.GetProductById(cartItem.ProductId);
                if (product == null || product.Quantity < cartItem.Quantity)
                {
                    TempData["ErrorMessage"] = $"Not enough stock for {cartItem.ProductName}";
                    return RedirectToAction("Index");
                }

                // Reduce stock quantity
                product.Quantity -= cartItem.Quantity;
                _context.Products.Update(product);
            }

            _context.SaveChanges();
            ClearCart(); // Empty cart after ordering

            return RedirectToAction("MyOrders", "Order");
        }

        private List<CartModel> GetCartItemsFromCookies()
        {
            var cookieValue = Request.Cookies["ShoppingCart"];
            if (string.IsNullOrEmpty(cookieValue))
                return new List<CartModel>();

            try
            {
                return JsonConvert.DeserializeObject<List<CartModel>>(cookieValue) ?? new List<CartModel>();
            }
            catch (Exception)
            {
                Response.Cookies.Delete("ShoppingCart");
                return new List<CartModel>();
            }
        }

        private void SaveCartItemsToCookies(List<CartModel> cartItems)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(3),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("ShoppingCart", JsonConvert.SerializeObject(cartItems), options);
        }
    }
}

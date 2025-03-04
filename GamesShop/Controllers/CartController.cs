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

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int ProductId, string ProductName, string Picture, decimal price, decimal discount)
        {
            // The service handles reading/writing the cart cookie.
            _cartService.AddToCart(ProductId);
            return RedirectToAction("Index", "Cart");
        }

        // GET: /Cart/Index
        public IActionResult Index()
        {
            // Build the cart items list by providing a function to retrieve product details.
            var cartItems = _cartService.GetCartItems();
            return View(cartItems);
        }
    }
}

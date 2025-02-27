using GamesShop.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

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
            var cart = _cartService.GetCartItems();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int ProductId, string ProductName, string Picture, int Quantity, decimal CurrentPrice, decimal CurrentDiscountPercentage, decimal TotalPrice)
        {
            // Use the provided values, or compute values as needed
            _cartService.AddToCart(ProductId, ProductName, Picture, CurrentPrice, CurrentDiscountPercentage);
            return RedirectToAction("Index");
        }


        public IActionResult RemoveFromCart(int productId)
        {
            _cartService.RemoveFromCart(productId);
            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            return RedirectToAction("Index");
        }
    }
}

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GamesShop.Core.Services;
using GamesShop.Core.Contracts;

namespace GamesShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: /Order/Create
        public async Task<IActionResult> Create()
        {
            // Get the current user's Id. Adjust as needed for your authentication.
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var order = await _orderService.CreateOrderAsync(userId);
                // You could redirect to a confirmation page or pass the order to a view.
                return RedirectToAction("Success", "Order");
            }
            catch (System.Exception ex)
            {
                // Handle error (e.g. log it) and provide feedback.
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Cart");
            }
        }

        public IActionResult Success()
        {
            return this.View();
        }
    }
}

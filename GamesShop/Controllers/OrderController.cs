using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GamesShop.Core.Services;
using GamesShop.Core.Contracts;
using GamesShop.Models.Order;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using GamesShop.Infrastructure.Data.Entities;
using GamesShop.Infrastructure.Enums;
using GamesShop.Infrastructure.Cart;

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
        [HttpPost]
        public async Task<IActionResult> Create(List<CartModel> cartItems)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var order = await _orderService.CreateOrderAsync(userId, cartItems);
                return RedirectToAction("Success", "Order");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Denied", "Order");
            }
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            List<OrderIndexVM> orders = _orderService.GetOrders()
                .Select(order => new OrderIndexVM
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate.ToString("dd-MMM-yyyy hh:mm", CultureInfo.InvariantCulture),
                    
                    UserId = order.UserId,
                    User = order.User.UserName,
                    TotalPrice = order.TotalPrice,
                    Status = order.Status.ToString(),

                    // Extract Order Items
                    OrderItems = order.OrderItems.Select(item => new OrderItemVM
                    {
                        ProductId = item.ProductId,
                        ProductName = item.Product.ProductName,
                        Picture = item.Product.Picture,
                        Producer = item.Product.Producer,
                        Description = item.Product.Description,
                        Quantity = item.Quantity,
                        Price = item.Price * item.Quantity,
                        
                    }).ToList()
                }).ToList();

            return View(orders);
        }

        public ActionResult MyOrders()
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<OrderIndexVM> orders = _orderService.GetOrdersByUser(currentUserId)
                .Select(order => new OrderIndexVM
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate.ToString("dd-MMM-yyyy hh:mm", CultureInfo.InvariantCulture),
                    
                    UserId = order.UserId,
                    User = order.User.UserName,
                    TotalPrice = order.TotalPrice,
                    Status = order.Status.ToString(), // Convert enum to string

                    // Flattening OrderItems into a single string representation (if needed)
                    OrderItems = order.OrderItems.Select(item => new OrderItemVM
                    {
                        ProductId = item.ProductId,
                        ProductName = item.Product.ProductName,
                        Picture = item.Product.Picture,
                        Producer = item.Product.Producer,
                        Description = item.Product.Description,
                        Quantity = item.Quantity,
                        Price = item.Price * item.Quantity,
                        
                    }).ToList()
                }).ToList();

            return View(orders);
        }
        [HttpPost]
        public IActionResult ChangeStatus(int orderId, OrderStatus newStatus)
        {
            var updated = _orderService.UpdateOrderStatus(orderId, newStatus);
            if (updated)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Status update failed");
        }





        public IActionResult Success()
        {
            return this.View();
        }

        public IActionResult Denied()
        {
            return this.View();
        }
    }
}

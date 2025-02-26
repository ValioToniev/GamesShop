using GamesShop.Core.Contracts;
using GamesShop.Infrastructure.Data.Entities;
using GamesShop.Infrastructure.Enums;
using GamesShop.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace GamesShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public OrderController(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }

        // GET: OrderController/Create
        public ActionResult Create(int id)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            // Ако има продукт с това id, го зареждаме във формата за поръчка
            OrderCreateVM order = new OrderCreateVM()
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                QuantityInStock = product.Quantity,
                Price = product.Price,
                Discount = product.Discount,
                Picture = product.Picture
            };

            return View(order);
        }
        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderCreateVM bindingModel)
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = this._productService.GetProductById(bindingModel.ProductId);
            if (currentUserId == null || product == null || product.Quantity < bindingModel.Quantity || product.Quantity == 0)
            {
                // Ако потребителят не съществува или продуктът не съществува или няма достатъчно наличност
                return RedirectToAction("Denied", "Order");
            }

            if (ModelState.IsValid)
            {
                _orderService.Create(bindingModel.ProductId, currentUserId, bindingModel.Quantity);
            }
            // При успешна поръчка се връща в списъка на продуктите
            return this.RedirectToAction("Index", "Product");
        }
        // GET: OrderController
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            // string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var user = context.Users.SingleOrDefault(u => u.Id == userId);

            List<OrderIndexVM> orders = _orderService.GetOrders()
                .Select(x => new OrderIndexVM
                {
                    Id = x.Id,
                    OrderDate = x.OrderDate.ToString("dd-MMM-YYYY hh:mm", CultureInfo.InvariantCulture),
                    UserId = x.UserId,
                    User = x.User.UserName,
                    ProductId = x.ProductId,
                    Product = x.Product.ProductName,
                    Picture = x.Product.Picture,
                    Quantity = x.Quantity,
                    Price = x.CurrentPrice,
                    Discount = x.CurrentDiscountPercentage,
                    TotalPrice = x.TotalPrice,
                    Status = x.Status,
                })
                .ToList();

            return View(orders);
        }

        public IActionResult Denied()
        {
            return View();
        }

        public ActionResult MyOrders()
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var user = context.Users.SingleOrDefault(u => u.Id == userId);

            List<OrderIndexVM> orders = _orderService.GetOrdersByUser(currentUserId)
                .Select(x => new OrderIndexVM
                {
                    Id = x.Id,
                    OrderDate = x.OrderDate.ToString("dd-MMM-yyyy hh:mm", CultureInfo.InvariantCulture),
                    UserId = x.UserId,
                    User = x.User.UserName,
                    ProductId = x.ProductId,
                    Product = x.Product.ProductName,
                    Picture = x.Product.Picture,
                    Quantity = x.Quantity,
                    Price = x.CurrentPrice,
                    Discount = x.CurrentDiscountPercentage,
                    TotalPrice = x.TotalPrice,
                }).ToList();

            return View(orders);
        }

        [Authorize(Roles = "Administrator")]
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






    }

}

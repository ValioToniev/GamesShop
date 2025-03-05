using GamesShop.Infrastructure.Enums;

namespace GamesShop.Models.Order
{
    
    
        public class OrderIndexVM
        {
            public int Id { get; set; }
            public string OrderDate { get; set; } = null!;
        public int ProductId { get; set; }
        public string Product { get; set; }
            public string UserId { get; set; } = null!;
            public string User { get; set; } = null!;

            public decimal TotalPrice { get; set; }
            public string Status { get; set; }

            // List of Order Items
            public List<OrderItemVM> OrderItems { get; set; } = new List<OrderItemVM>();
        }

    
}

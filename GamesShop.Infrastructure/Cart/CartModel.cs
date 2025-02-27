using System.ComponentModel.DataAnnotations;

namespace GamesShop.Infrastructure.Cart
{
    public class CartModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Picture { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal CurrentPrice { get; set; }

        [Range(0, 100)]
        public decimal CurrentDiscountPercentage { get; set; }

        public decimal TotalPrice{ get; set; }
        
    }
}

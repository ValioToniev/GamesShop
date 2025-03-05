namespace GamesShop.Models.Order
{
    public class OrderItemVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Picture { get; set; } = null!;
        public string Producer { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }

}

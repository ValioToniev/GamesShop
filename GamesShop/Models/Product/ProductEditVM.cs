using GamesShop.Models.Category;
using GamesShop.Models.Genre;
using System.ComponentModel.DataAnnotations;

namespace GamesShop.Models.Product
{
    public class ProductEditVM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = null!;

        [Required]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }
        public virtual List<GenrePairVM> Genres { get; set; } = new List<GenrePairVM>();

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public virtual List<CategoryPairVM> Categories { get; set; } = new List<CategoryPairVM>();

        [Required]
        [Display(Name = "Producer")]
        public string Producer { get; set; }

        [Display(Name = "Picture")]
        public string Picture { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Range(0, 5000)]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Discount")]
        public decimal Discount { get; set; }


    }
}

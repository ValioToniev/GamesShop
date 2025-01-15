using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Infrastructure.Data.Entities
{
    
    
        public class Order
        {
            public int Id { get; set; }

            [Required]
            public DateTime OrderDate { get; set; }

            [Required]
            [ForeignKey(nameof(Product))]
            public int ProductId { get; set; }
            public virtual Product Product { get; set; } = null!;

            [Required]
            [ForeignKey(nameof(ApplicationUser))]
            public string UserId { get; set; } = null!;
            public virtual ApplicationUser User { get; set; } = null!;

            [Required]
            public int Quantity { get; set; }

            [Required]
            public decimal CurrentPrice { get; set; }

            [Range(0,100)]
            [Required]
            public decimal CurrentDiscountPercentage { get; set; }

            public decimal TotalPrice
            {
                get
                {
                    return Quantity * CurrentPrice * (1 - CurrentDiscountPercentage / 100);
                }
            }
        }

    
}

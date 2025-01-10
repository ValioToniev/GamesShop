using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Infrastructure.Data.Entities
{
    public class Address
    {        
        [Key]
        public int AddressId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Country { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string Neighborhood { get; set; }
        [Required]
        [MaxLength(100)]
        public string StreetNumber { get; set; }
        [Required]
        public int Number { get; set; }
        
        


    }
}

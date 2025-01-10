using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Infrastructure.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(30)]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [MaxLength(30)]
        public string FirstName {  get; set; }
        [Required]
        [MaxLength(30)]  
        public string LastName { get; set; }
        [Required]
        public string Email {  get; set; }
        [Required]
        [RegularExpression(@"^\+[1-9]\d{1,14}$", ErrorMessage = "Невалиден телефонен номер.")]
        public string Phone { get; set; }
        [Required]
        public Address Address { get; set; }


    }
}

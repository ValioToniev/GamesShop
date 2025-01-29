using System.ComponentModel.DataAnnotations;

namespace GamesShop.Models.Genre
{
    public class GenrePairVM
    {
        public int Id { get; set; }

        [Display(Name = "Genre")]
        public string Name { get; set; } = null!;
    }
}

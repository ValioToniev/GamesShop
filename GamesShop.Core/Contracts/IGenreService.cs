using GamesShop.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Core.Contracts
{
    public interface IGenreService
    {
        List<Genre> GetGenres();
        Genre GetGenreById(int genreId);
        List<Product> GetProductsByGenre(int genreId);
    }

}

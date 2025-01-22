using GamesShop.Core.Contacts;
using GamesShop.Infrastructure.Data.Entities;
using GamesShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Core.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _context;

        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Genre> GetGenres()
        {
            List<Genre> genres = _context.Genres.ToList();
            return genres;
        }

        public Genre GetGenreById(int genreId)
        {
            return _context.Genres.Find(genreId);
        }

        public List<Product> GetProductsByGenre(int genreId)
        {
            return _context.Products.Where(x => x.GenreId == genreId).ToList();
        }
    }

}

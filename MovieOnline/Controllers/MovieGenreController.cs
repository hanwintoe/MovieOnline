using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieOnline.Data;
using MovieOnline.Models.MovieModels;

namespace MovieOnline.Controllers
{
    public class MovieGenreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieGenreController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Movie.ToList());
        }

        //GET
        public IActionResult AddGenre(int? id)
        {
            MovieDto model = new();
            List<int> GenreIds = new();

            if (id.HasValue)
            {
                var movie = _context.Movie.Include("MovieGenres").FirstOrDefault(x => x.Id == id);
                movie.MovieGenres.ToList().ForEach(result => GenreIds.Add(result.MovieId));

                //bind model
                model.Title = movie.Title;
                model.GenreIds = GenreIds.ToArray();
                model.DrpGenres = _context.Genre.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString()}).ToList();

            }
            else
            {
                model.DrpGenres = _context.Genre.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            }
            return View(model);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGenre(MovieDto modle)
        {
            Movie movie = new();
            List<MovieGenre> movieGenres = new();

            if(modle.Id > 0)
            {
                movie = _context.Movie.Include("MovieGenres").FirstOrDefault(x => x.Id == modle.Id);
                movie.MovieGenres.ToList().ForEach(result => movieGenres.Add(result));
                _context.MovieGenre.RemoveRange(movieGenres);
                _context.SaveChanges();

                //update Movie Genre details
                movie.Title = modle.Title;
                if(modle.GenreIds.Length > 0)
                {
                    movieGenres = new List<MovieGenre>();
                    foreach(var genreId in modle.GenreIds)
                    {
                        movieGenres.Add(new MovieGenre { GenreId = genreId, MovieId = modle.Id });
                    }
                    movie.MovieGenres = movieGenres;
                }
                _context.SaveChanges();
            }
            else
            {
                movie.Title = modle.Title;
                if (modle.GenreIds.Length > 0)
                {
                    movieGenres = new List<MovieGenre>();
                    foreach (var genreId in modle.GenreIds)
                    {
                        movieGenres.Add(new MovieGenre { GenreId = genreId, MovieId = modle.Id });
                    }
                    movie.MovieGenres = movieGenres;
                }
                _context.Movie.Add(movie);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}

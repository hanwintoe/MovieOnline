using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MovieOnline.Data;
using MovieOnline.Models.MovieModels;
using MovieOnline.ViewModels;

namespace MovieOnline.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public MovieController(ApplicationDbContext context, IWebHostEnvironment _webHostEnvironment)
        {
            _context = context;
            webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(_context.Movie.ToList());
        }

        [HttpPost]
        //Rating Voter
        public async Task<JsonResult> PostRating(int rating, int mid)
        {
            var movie = _context.Movie.FindAsync(mid);
            movie.Result.RateCount += 1;
            movie.Result.RateTotal += rating;

            _context.Movie.Update(await movie);
            await _context.SaveChangesAsync();

            return Json(" You rate this " + rating.ToString() + " star(s)");
        }

        //GET
        public IActionResult Create() => View();

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieViewModel model)
        {
            List<int> GenreIds = new();
            if (ModelState.IsValid)
            {
                string hiLightFileName = HilightUploadedFile(model);
                string viewFileName = ViewUploadedFile(model);
                string bannerFileName = BannerUploadedFile(model);

                Movie movie = new()
                {
                    Title = model.Title,
                    ReleaseDate = model.ReleaseDate,
                    ImageHiLight = hiLightFileName,
                    ImageView = viewFileName,
                    ImageBanner = bannerFileName
                    

                };
                _context.Movie.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(model);

        }

        //GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            var movieViewModel = new MovieViewModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                ExistingImageHiLight = movie.ImageHiLight,
                ExistingImageView = movie.ImageView,
                ExistingImageBanner = movie.ImageBanner,
            };
            if (movie == null)
                return NotFound();

            return View(movieViewModel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieViewModel model)
        {
            if (ModelState.IsValid)
            {
                var movie = await _context.Movie.FindAsync(model.Id);
                movie.Title = model.Title;
                movie.ReleaseDate = model.ReleaseDate;

                if (model.MovieImageHiLight != null)
                {
                    if (movie.ImageHiLight != null)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", movie.ImageHiLight);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                if (model.MovieImageView != null)
                {
                    if (movie.ImageView != null)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", movie.ImageView);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                if (model.MovieImageBanner != null)
                {
                    if (movie.ImageBanner != null)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", movie.ImageBanner);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }

                movie.ImageHiLight = HilightUploadedFile(model);
                movie.ImageView = ViewUploadedFile(model);
                movie.ImageBanner = BannerUploadedFile(model);
                _context.Update(movie);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        //GET
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
                return NotFound();

            return View(movie);
        }

        //GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var movie = await _context.Movie.FindAsync(id);
            var movieViewModel = new MovieViewModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                ExistingImageHiLight = movie.ImageHiLight,
                ExistingImageView = movie.ImageView,
                ExistingImageBanner = movie.ImageBanner
            };
            if (movie == null)
            {
                return NotFound();
            }
            return View(movieViewModel);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            var CurrentImageHilight = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", movie.ImageHiLight);
            var CurrentImageView = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", movie.ImageView);
            var CurrentImageBanner = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", movie.ImageBanner);
            _context.Movie.Remove(movie);
            if (await _context.SaveChangesAsync() > 0)
            {
                if (System.IO.File.Exists(CurrentImageHilight))
                {
                    System.IO.File.Delete(CurrentImageHilight);
                }
                if (System.IO.File.Exists(CurrentImageView))
                {
                    System.IO.File.Delete(CurrentImageView);
                }
                if (System.IO.File.Exists(CurrentImageBanner))
                {
                    System.IO.File.Delete(CurrentImageBanner);
                }
            };
            return RedirectToAction(nameof(Index));

        }



        //For Movie Images Loading 
        private string HilightUploadedFile(MovieViewModel model)
        {
            string uniqueFileName = null;
            if (model.MovieImageHiLight != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.MovieImageHiLight.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.MovieImageHiLight.CopyTo(fileStream);

            }
            return uniqueFileName;
        }

        private string ViewUploadedFile(MovieViewModel model)
        {
            string uniqueFileName = null;
            if (model.MovieImageView != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.MovieImageView.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.MovieImageView.CopyTo(fileStream);

            }
            return uniqueFileName;
        }

        private string BannerUploadedFile(MovieViewModel model)
        {
            string uniqueFileName = null;
            if (model.MovieImageBanner != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.MovieImageBanner.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.MovieImageBanner.CopyTo(fileStream);

            }
            return uniqueFileName;
        }


    }
}

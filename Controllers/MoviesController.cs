using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WhatMovie.Data;
using WhatMovie.Models.Database;
using WhatMovie.Models.Settings;
using WhatMovie.Services.Interfaces;

namespace WhatMovie.Controllers
{
    [Route("[controller]")]
    public class MoviesController : Controller
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IRemoteMovieService _tmdbMovieService;
        private readonly IDataMappingService _tmdbMappingService;

        public MoviesController(ILogger<MoviesController> logger, IOptions<AppSettings> appSettings, ApplicationDbContext context, IImageService imageService, IRemoteMovieService tmdbMovieService, IDataMappingService tmdbMappingService)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _context = context;
            _imageService = imageService;
            _tmdbMovieService = tmdbMovieService;
            _tmdbMappingService = tmdbMappingService;
        }

        public async Task<IActionResult> Import()
        {
            var movies = await _context.Movies!.ToListAsync();

            return View(movies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(int id)
        {
            // If we already have the movie we can warn the user instead of importing it again
            if (_context.Movies!.Any(m => m.MovieId == id))
            {
                var localMovie = await _context.Movies!.FirstOrDefaultAsync(m => m.MovieId == id);
                return RedirectToAction("Details", "Movies", new { id = localMovie?.Id, local = true});
            }

            // Get the raw data from the API
            var movieDetail = await _tmdbMovieService.MovieDetailAsync(id);

            // Run the data through the mapping procedure
            var movie = await _tmdbMappingService.MapMovieDetailAsync(movieDetail);

            // Add the new movie to the database
            _context.Add(movie);
            await _context.SaveChangesAsync();

            // Assign movie to the default All collection
            await AddToMovieCollection(movie.Id, _appSettings.WhatMovieSettings!.DefaultCollection!.Name!);

            return RedirectToAction("Import");
        }

        private async Task AddToMovieCollection(int movieId, string collectionName)
        {
            var collection = await _context.Collections!.FirstOrDefaultAsync(c => c.Name == collectionName);

            _context.Add(
                new MovieCollection()
                {
                    CollectionId = collection!.Id,
                    MovieId = movieId
                }
            );

            await _context.SaveChangesAsync();
        }

        private async Task AddToMovieCollection(int movieId, int collectionId)
        {
            _context.Add(
                new MovieCollection()
                {
                    CollectionId = collectionId,
                    MovieId = movieId,
                }
            );

            await _context.SaveChangesAsync();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
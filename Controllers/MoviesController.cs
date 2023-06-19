using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Library()
        {
            var movies = await _context.Movies!.ToListAsync();
            return View(movies);
        }

        // GET: Movie/Create
        public IActionResult Create()
        {
            ViewData["CollectionId"] = new SelectList(_context.Collections, "Id", "Name");
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MovieId,Title,Tagline,Overview,Runtime,ReleaseDate,VoteAverage,TrailerUrl")] Movie movie, int collectionId)
        {
            if (ModelState.IsValid)
            {
                movie.PosterType = movie.PosterFile?.ContentType;
                movie.Poster = await _imageService.EncodeImageAsync(movie.PosterFile!);

                movie.BackdropType = movie.BackdropFile?.ContentType;
                movie.Backdrop = await _imageService.EncodeImageAsync(movie.BackdropFile!);

                _context.Add(movie);
                await _context.SaveChangesAsync();

                await AddToMovieCollection(movie.Id, collectionId);

                return RedirectToAction("Index", "MovieCollections");
            }

            return View(movie);
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies!.FindAsync(id);

            if (movie == null) return NotFound();

            return View();
        }

        // POST: Movie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,MovieId,Title,Tagline,Overview,Runtime,ReleaseDate,VoteAverage,TrailerUrl")] Movie movie)
        {
            if (id != movie.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (movie.PosterFile is not null)
                    {
                        movie.PosterType = movie.PosterFile.ContentType;
                        movie.Poster = await _imageService.EncodeImageAsync(movie.PosterFile);
                    }

                    if (movie.BackdropFile is not null)
                    {
                        movie.BackdropType = movie.BackdropFile.ContentType;
                        movie.Backdrop = await _imageService.EncodeImageAsync(movie.BackdropFile);
                    }

                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id)) return NotFound();
                    else throw;
                }

                return RedirectToAction("Details", "Movies", new { id = movie.Id, local = true });
            }

            return View(movie);
        }

        // GET: Movie/Delete=5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies!
                .FirstOrDefaultAsync(movie => movie.Id == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        //POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies!.FindAsync(id);

            _context.Movies.Remove(movie!);
            await _context.SaveChangesAsync();

            return RedirectToAction("Library", "Movies");
        }

        public async Task<IActionResult> Details(int? id, bool local = false)
        {
            if (id == null || _context.Movies == null) return NotFound();

            Movie? movie = new();

            if (local)
            {
                // Get the movie data from the database
                movie = await _context.Movies
                    .Include(movie => movie.Cast)
                    .Include(movie => movie.Crew)
                    .FirstOrDefaultAsync(movie => movie.Id == id);
            }
            else
            {
                // Get the movie data from the TMDB API
                var movieDetail = await _tmdbMovieService.MovieDetailAsync((int)id);
                movie = await _tmdbMappingService.MapMovieDetailAsync(movieDetail);
            }

            if (movie == null) return NotFound();

            ViewData["Local"] = local;

            return View(movie);
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

        private bool MovieExists(int id)
        {
            return _context.Movies!.Any(m => m.Id == id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
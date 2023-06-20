using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhatMovie.Data;
using WhatMovie.Models.Database;

namespace WhatMovie.Controllers
{
    [Route("[controller]")]
    public class MovieCollectionsController : Controller
    {
        private readonly ILogger<MovieCollectionsController> _logger;
        private readonly ApplicationDbContext _context;

        public MovieCollectionsController(ILogger<MovieCollectionsController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            id ??= (await _context.Collections!.FirstOrDefaultAsync(c => c.Name!.ToUpper() == "ALL"))!.Id;

            ViewData["CollectionList"] = new SelectList(_context.Collections, "Id", "Name", id);

            var allMovieIds = await _context.Movies!.Select(m => m.Id).ToListAsync();

            var movieIdsInCollection = await _context.MovieCollection!
                .Where(m => m.CollectionId == id)
                .OrderBy(m => m.Order)
                .Select(m => m.MovieId)
                .ToListAsync();

            var movieIdsNotInCollection = allMovieIds.Except(movieIdsInCollection);

            var moviesInCollection = new List<Movie>();
            movieIdsInCollection.ForEach(movieId => moviesInCollection.Add(
                _context.Movies?.Find(movieId)!
            ));

            ViewData["MoviesInCollection"] = new MultiSelectList(moviesInCollection, "Id", "Title");

            var moviesNotInCollection = await _context.Movies!.AsNoTracking()
                .Where(m => movieIdsNotInCollection.Contains(m.Id)).ToListAsync();

            ViewData["MoviesNotInCollection"] = new MultiSelectList(moviesNotInCollection, "Id", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id, List<int> idsInCollection)
        {
            if (_context.MovieCollection != null)
            {
                // Remove old movie records from collection
                var oldRecords = _context.MovieCollection.Where(c => c.CollectionId == id);
                _context.MovieCollection.RemoveRange(oldRecords);
                await _context.SaveChangesAsync();
            }

            if (idsInCollection != null)
            {
                int index = 1;

                idsInCollection.ForEach(movieId =>
                {
                    _context.Add(new MovieCollection()
                    {
                        CollectionId = id,
                        MovieId = movieId,
                        Order = index++,
                    });
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
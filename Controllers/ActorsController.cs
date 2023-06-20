using Microsoft.AspNetCore.Mvc;
using WhatMovie.Services.Interfaces;

namespace WhatMovie.Controllers
{
    [Route("[controller]")]
    public class ActorsController : Controller
    {
        private readonly ILogger<ActorsController> _logger;
        private readonly IRemoteMovieService _tmdbMovieService;
        private readonly IDataMappingService _mappingService;

        public ActorsController(ILogger<ActorsController> logger, IRemoteMovieService tmdbMovieService, IDataMappingService mappingService)
        {
            _logger = logger;
            _tmdbMovieService = tmdbMovieService;
            _mappingService = mappingService;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var actor = await _tmdbMovieService.ActorDetailAsync(id);
            actor = _mappingService.MapActorDetail(actor);

            return View(actor);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
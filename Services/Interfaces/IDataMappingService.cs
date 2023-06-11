using WhatMovie.Models.Database;
using WhatMovie.Models.TMDB;

namespace WhatMovie.Services.Interfaces
{
    public interface IDataMappingService
    {
        Task<Movie> MapMovieDetailAsync(MovieDetail movie);
        ActorDetail MapActorDetail(ActorDetail actor);

    }
}
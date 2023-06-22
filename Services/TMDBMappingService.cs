using WhatMovie.Enums;
using WhatMovie.Models.Database;
using WhatMovie.Models.Settings;
using WhatMovie.Models.TMDB;
using WhatMovie.Services.Interfaces;

namespace WhatMovie.Services
{
    public class TMDBMappingService : IDataMappingService
    {
        private readonly AppSettings _appSettings;
        private readonly IImageService _imageService;

        public TMDBMappingService(AppSettings appSettings, IImageService imageService)
        {
            _appSettings = appSettings;
            _imageService = imageService;
        }

        public ActorDetail MapActorDetail(ActorDetail actor)
        {
            // Image
            actor.ProfilePath = BuildCastImage(actor.ProfilePath);

            // Bio
            if (string.IsNullOrEmpty(actor.Biography))
                actor.Biography = "Not available.";

            // Place of birth
            if (string.IsNullOrEmpty(actor.PlaceOfBirth))
                actor.PlaceOfBirth = "Not available.";

            // Birthday
            if (string.IsNullOrEmpty(actor.Birthday))
                actor.Birthday = "Not available.";
            else
                actor.Birthday = DateTime.Parse(actor.Birthday).ToString("MMM dd, yyyy");

            return actor;
        }

        public async Task<Movie> MapMovieDetailAsync(MovieDetail movie)
        {
            Movie? newMovie = null;

            try
            {
                newMovie = new Movie()
                {
                    MovieId = movie.Id,
                    Title = movie.Title,
                    Tagline = movie.Tagline,
                    Overview = movie.Overview,
                    Runtime = movie.Runtime,
                    VoteAverage = movie.VoteAverage,
                    ReleaseDate = DateTime.Parse(movie.ReleaseDate),
                    TrailerUrl = BuildTrailerPath(movie.Videos),
                    Backdrop = await EncodeBackdropImageAsync(movie.BackdropPath),
                    BackdropType = BuildImageType(movie.BackdropPath),
                    Poster = await EncodePosterImageAsync(movie.PosterPath),
                    PosterType = BuildImageType(movie.PosterPath),
                    Rating = TMDBMappingService.GetRating(movie.ReleaseDates)
                };

                var castMembers = movie.Credits?.Cast?.OrderByDescending(c => c.Popularity)
                    .GroupBy(c => c.CastId)
                    .Select(g => g.First())
                    .Take(20)
                    .ToList();

                castMembers.ForEach(member =>
                {
                    newMovie.Cast.Add(new MovieCast()
                    {
                        CastId = member.Id,
                        Department = member.KnownForDepartment,
                        Name = member.Name,
                        Character = member.Character,
                        ImageUrl = BuildCastImage(member.ProfilePath)
                    });
                });

                var crewMembers = movie.Credits?.Crew?.OrderByDescending(c => c.Popularity)
                    .GroupBy(c => c.Id)
                    .Select(g => g.First())
                    .Take(20)
                    .ToList();

                crewMembers.ForEach(member =>
                {
                    newMovie.Crew.Add(new MovieCrew()
                    {
                        CrewId = member.Id,
                        Department = member.Department,
                        Name = member.Name,
                        Job = member.Job,
                        ImageUrl = BuildCastImage(member.ProfilePath)
                    });
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in MapMovieDetailAsync: {ex.Message}");
            }

            return newMovie!;
        }

        private string BuildCastImage(string? path)
        {
            if (string.IsNullOrEmpty(path))
                return _appSettings.WhatMovieSettings!.DefaultCastImage!;

            return $"{_appSettings.TMDBSettings!.BaseImagePath}/{_appSettings.WhatMovieSettings!.DefaultPosterSize}/{path}";
        }

        private static MovieRating GetRating(ReleaseDates? dates)
        {
            var movieRating = MovieRating.NR;
            var certification = dates?.Results?.FirstOrDefault(r => r.Iso3166_1 == "US");
            
            if (certification is not null)
            {
                var apiRating = certification.ReleaseDates?.FirstOrDefault(c => c.Certification != "")?.Certification?.Replace("-", "");

                if (!string.IsNullOrEmpty(apiRating))
                {
                    movieRating = (MovieRating)Enum.Parse(typeof(MovieRating), apiRating, true);
                }
            }

            return movieRating;
        }

        private async Task<byte[]> EncodePosterImageAsync(string? path)
        {
            var posterPath = $"{_appSettings.TMDBSettings?.BaseImagePath}/{_appSettings.WhatMovieSettings?.DefaultPosterSize}/{path}";
            return await _imageService.EncodeImageUrlAsync(posterPath);
        }

        private static string BuildImageType(string? path)
        {
            if (string.IsNullOrEmpty(path)) return path!;

            return $"image/{Path.GetExtension(path).TrimStart('.')}";
        }

        private async Task<byte[]> EncodeBackdropImageAsync(string? path)
        {
            var backdropPath = $"{_appSettings.TMDBSettings?.BaseImagePath}/{_appSettings.WhatMovieSettings?.DefaultBackdropSize}/{path}";
            return await _imageService.EncodeImageUrlAsync(backdropPath);
        }

        private string? BuildTrailerPath(Videos videos)
        {
            var videoKey = videos.Results.FirstOrDefault(r => r.Type.ToLower().Trim() == "trailer" && r.Key != "").Key;
            return string.IsNullOrEmpty(videoKey) ? videoKey : $"{_appSettings.TMDBSettings!.BaseYouTubePath}{videoKey}";
        }
    }
}
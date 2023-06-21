using System.Runtime.Serialization.Json;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using WhatMovie.Enums;
using WhatMovie.Models.Settings;
using WhatMovie.Models.TMDB;
using WhatMovie.Services.Interfaces;

namespace WhatMovie.Services
{
    public class TMDBMovieService : IRemoteMovieService
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        public TMDBMovieService(IOptions<AppSettings> appSettings, IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _appSettings = appSettings.Value;
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<ActorDetail> ActorDetailAsync(int id)
        {
            // Assemble a full request uri string
            var query = $"{_appSettings.TMDBSettings!.BaseUrl}/person/{id}";
            
            var queryParams = new Dictionary<string, string>()
            {
                { "api_key", _configuration.GetSection("ApiSettings")["TMDBApiKey"]! },
                { "language", _appSettings.TMDBSettings.QueryOptions!.Language! },
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams!);

            // Create a client and execute the request
            var client = _httpClient.CreateClient();
            using var response = await client.GetAsync(requestUri);

            // Return the ActorDetail object
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var actorDetail = await JsonSerializer.DeserializeAsync<ActorDetail>(responseStream);

                if (actorDetail is not null) return actorDetail;
            }

            return new ActorDetail();
        }

        public async Task<MovieDetail> MovieDetailAsync(int id)
        {
            // Assemble a full request uri string
            var query = $"{_appSettings.TMDBSettings!.BaseUrl}/movie/{id}";
            
            var queryParams = new Dictionary<string, string>()
            {
                { "api_key", _configuration.GetSection("ApiSettings")["TMDBApiKey"]! },
                { "language", _appSettings.TMDBSettings.QueryOptions!.Language! },
                { "append_to_response", _appSettings.TMDBSettings.QueryOptions.AppendToResponse! }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams!);

            // Create a client and execute the request
            using var client = _httpClient.CreateClient();
            using var response = await client.GetAsync(requestUri);

            // Return the MovieDetail object
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var movieDetail = await JsonSerializer.DeserializeAsync<MovieDetail>(responseStream);

                if (movieDetail is not null) return movieDetail;
            }

            return new MovieDetail();
        }

        public async Task<MovieSearch> SearchMovieAsync(MovieCategory category, int count)
        {
            // Assemble a full request uri string
            var query = $"{_appSettings.TMDBSettings!.BaseUrl}/movie/{category}";
            
            var queryParams = new Dictionary<string, string>()
            {
                { "api_key", _configuration.GetSection("ApiSettings")["TMDBApiKey"]! },
                { "language", _appSettings.TMDBSettings.QueryOptions!.Language! },
                { "page", _appSettings.TMDBSettings.QueryOptions.Page! }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams!);

            // Create a client and execute the request
            using var client = _httpClient.CreateClient();
            using var response = await client.GetAsync(requestUri);

            // Return the MovieSearch object
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var movieSearch = await JsonSerializer.DeserializeAsync<MovieSearch>(responseStream);

                if (movieSearch is not null)
                {
                    movieSearch.Results = movieSearch.Results?.Take(count).ToArray();
                    movieSearch.Results?.ToList().ForEach(r => r.PosterPath = $"{_appSettings.TMDBSettings.BaseImagePath}/{_appSettings.WhatMovieSettings!.DefaultPosterSize}/{r.PosterPath}");
                
                    return movieSearch;
                }
            }

            return new MovieSearch();
        }
    }
}
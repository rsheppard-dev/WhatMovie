using System.Runtime.Serialization.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Build.Framework;
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
            // Setup a default instance of ActorDetail
            ActorDetail actorDetail = new();

            // Assemble a full request uri string
            var query = $"{_appSettings.TMDBSettings!.BaseUrl}/person/{id}";
            
            var queryParams = new Dictionary<string, string>()
            {
                { "api_key", _configuration.GetSection("ApiSettings")["TMDBApiKey"]! },
                { "language", _appSettings.TMDBSettings.QueryOptions!.Language! },
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams!);

            // Create a client and exexute the request
            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await client.SendAsync(request);

            // Return the ActorDetail object
            if (response.IsSuccessStatusCode)
            {
                var dcjs = new DataContractJsonSerializer(typeof(ActorDetail));
                using var responseStream = await response.Content.ReadAsStreamAsync();
                actorDetail = (ActorDetail)dcjs.ReadObject(responseStream)!;
            }

            return actorDetail;
        }

        public async Task<MovieDetail> MovieDetailAsync(int id)
        {
            // Setup a default instance of MovieDetail
            MovieDetail movieDetail = new();

            // Assemble a full request uri string
            var query = $"{_appSettings.TMDBSettings!.BaseUrl}/movie/{id}";
            
            var queryParams = new Dictionary<string, string>()
            {
                { "api_key", _configuration.GetSection("ApiSettings")["TMDBApiKey"]! },
                { "language", _appSettings.TMDBSettings.QueryOptions!.Language! },
                { "append_to_response", _appSettings.TMDBSettings.QueryOptions.AppendToResponse! }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams!);

            // Create a client and exexute the request
            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await client.SendAsync(request);

            // Return the MovieDetail object
            if (response.IsSuccessStatusCode)
            {
                var dcjs = new DataContractJsonSerializer(typeof(MovieDetail));
                using var responseStream = await response.Content.ReadAsStreamAsync();
                movieDetail = (MovieDetail)dcjs.ReadObject(responseStream)!;
            }

            return movieDetail!;
        }

        public async Task<MovieSearch> SearchMovieAsync(MovieCategory category, int count)
        {
            // Setup a default instance of MovieSearch
            MovieSearch movieSearch = new();

            // Assemble a full request uri string
            var query = $"{_appSettings.TMDBSettings!.BaseUrl}/movie/{category}";
            
            var queryParams = new Dictionary<string, string>()
            {
                { "api_key", _configuration.GetSection("ApiSettings")["TMDBApiKey"]! },
                { "language", _appSettings.TMDBSettings.QueryOptions!.Language! },
                { "page", _appSettings.TMDBSettings.QueryOptions.Page! }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams!);

            // Create a client and exexute the request
            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await client.SendAsync(request);

            // Return the MovieSearch object
            if (response.IsSuccessStatusCode)
            {
                var dcjs = new DataContractJsonSerializer(typeof(MovieSearch));
                using var responseStream = await response.Content.ReadAsStreamAsync();
                
                movieSearch = (MovieSearch)dcjs.ReadObject(responseStream)!;
                movieSearch.Results = movieSearch.Results?.Take(count).ToArray();
                movieSearch.Results?.ToList().ForEach(r => r.PosterPath = $"{_appSettings.TMDBSettings.BaseImagePath}/{_appSettings.WhatMovieSettings.DefaultPosterSize}/{r.PosterPath}");
            }

            return movieSearch;
        }
    }
}
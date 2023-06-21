namespace WhatMovie.Models.Settings
{
    public class AppSettings
    {
        public WhatMovieSettings? WhatMovieSettings { get; set; }
        public TMDBSettings? TMDBSettings { get; set; }
        public ApiSettings? ApiSettings { get; set; }
        public DefaultCredentials? DefaultCredentials { get; set; }
    }

    public class ApiSettings
    {
        public string? TMDBApiKey { get; set; }
        public string? TMDBReadAccessToken { get; set; }
    }
    public class DefaultCredentials
    {
        public string? Role { get; set;}
        public string? Email { get; set; }
        public string? Password { get; set;}

    }
}
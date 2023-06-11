namespace WhatMovie.Models.Settings
{
    public class WhatMovieSettings
    {
        public string? TMDBApiKey { get; set; }
        public string? DefaultBackdropSize { get; set; }
        public string? DefaultPosterSize { get; set; }
        public string? DefaultYouTubeKey { get; set; }
        public string? DefaultCastImage { get; set; }
        public DefaultCollection? DefaultCollection { get; set; }
        public DefaultCredentials? DefaultCredentials { get; set; }
    }

    public class DefaultCollection
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class DefaultCredentials
    {
        public string? DefaultRole { get; set; }
        public string? DefaultEmail { get; set; }
        public string? DefaultPassword { get; set; }
    }
}
namespace WhatMovie.Models.TMDB
{
    public partial class MovieSearch
    {
        public int Page { get; set; }
        public MovieSearchResult[]? Results { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }

    public partial class MovieSearchResult
    {
        public bool Adult { get; set; }
        public string? BackdropPath { get; set; }
        public int[]? GenreIds { get; set; }
        public long Id { get; set; }
        public string? OriginalLanguage { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Overview { get; set; }
        public float Popularity { get; set; }
        public string? PosterPath { get; set; }
        public string? ReleaseDate { get; set; }
        public string? Title { get; set; }
        public bool Video { get; set; }
        public float VoteAverage { get; set; }
        public int VoteCount { get; set; }
    }
}
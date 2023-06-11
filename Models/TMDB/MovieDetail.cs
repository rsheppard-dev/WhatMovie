namespace WhatMovie.Models.TMDB
{
    public class MovieDetail
    {
        public bool Adult { get; set; }
        public string? BackdropPath { get; set; }
        public BelongsToCollection? BelongsToCollection { get; set; }
        public int Budget { get; set; }
        public Genre[]? Genres { get; set; }
        public string? Homepage { get; set; }
        public int Id { get; set; }
        public string? ImdbId { get; set; }
        public string? OriginalLanguage { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Overview { get; set; }
        public float Popularity { get; set; }
        public string? PosterPath { get; set; }
        public ProductionCompany[]? ProductionCompanies { get; set; }
        public ProductionCountry[]? ProductionCountries { get; set; }
        public string? ReleaseDate { get; set; }
        public int Revenue { get; set; }
        public int Runtime { get; set; }
        public SpokenLanguage[]? SpokenLanguages { get; set; }
        public string? Status { get; set; }
        public string? Tagline { get; set; }
        public string? Title { get; set; }
        public bool Video { get; set; }
        public float VoteAverage { get; set; }
        public float VoteCount { get; set; }
        public Credits? Credits { get; set; }
        public Images? Images { get; set; }
        public Videos? Videos { get; set; }
        public ReleaseDates? ReleaseDates { get; set; }
    }

    public class BelongsToCollection
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PosterPath { get; set; }
        public string? BackdropPath { get; set; }
    }

    public class Credits
    {
        public Cast[]? Cast { get; set; }
        public Cast[]? Crew { get; set; }
    }

    public class Cast
    {
        public bool Adult { get; set; }
        public int Gender { get; set; }
        public int Id { get; set; }
        public string? KnownForDepartment { get; set; }
        public string? Name { get; set; }
        public string? OriginalName { get; set; }
        public float Popularity { get; set; }
        public string? ProfilePath { get; set; }
        public int? CastId { get; set; }
        public string? Character { get; set; }
        public string? CreditId { get; set; }
        public float? Order { get; set; }
        public string? Department { get; set; }
        public string? Job { get; set; }
    }

    public class Genre
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class Images
    {
        public object[]? Backdrops { get; set; }
        public object[]? Logos { get; set; }
        public object[]? Posters { get; set; }
    }

    public class ProductionCompany
    {
        public int Id { get; set; }
        public string? LogoPath { get; set; }
        public string? Name { get; set; }
        public string? OriginCountry { get; set; }
    }

    public class ProductionCountry
    {
        public string? Iso3166_1 { get; set; }
        public string? Name { get; set; }
    }

    public class ReleaseDates
    {
        public ReleaseDatesResult[]? Results { get; set; }
    }

    public class ReleaseDatesResult
    {
        public string? Iso3166_1 { get; set; }

        public ReleaseDate[]? ReleaseDates { get; set; }
    }

    public class ReleaseDate
    {
        public string? Certification { get; set; }
        public object[]? Descriptors { get; set; }
        public string? Iso639_1 { get; set; }
        public string? Note { get; set; }
        public string? ReleaseDateReleaseDate { get; set; }
        public int Type { get; set; }
    }

    public class SpokenLanguage
    {
        public string? EnglishName { get; set; }
        public string? Iso639_1 { get; set; }
        public string? Name { get; set; }
    }

    public class Videos
    {
        public VideoResult[]? Results { get; set; }
    }

    public partial class VideoResult
    {
        public string? Iso639_1 { get; set; }
        public string? Iso3166_1 { get; set; }
        public string? Name { get; set; }
        public string? Key { get; set; }
        public string? Site { get; set; }
        public int Size { get; set; }
        public string? Type { get; set; }
        public bool Official { get; set; }
        public string? PublishedAt { get; set; }
        public string? Id { get; set; }
    }
}
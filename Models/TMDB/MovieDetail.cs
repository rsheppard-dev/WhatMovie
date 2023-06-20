using System.Text.Json.Serialization;

namespace WhatMovie.Models.TMDB
{
    public class MovieDetail
    {
        [JsonPropertyName("adult")]
        public bool Adult { get; set; }

        [JsonPropertyName("backdrop_path")]
        public string? BackdropPath { get; set; }

        [JsonPropertyName("belongs_to_collection")]
        public BelongsToCollection? BelongsToCollection { get; set; }

        [JsonPropertyName("budget")]
        public int Budget { get; set; }

        [JsonPropertyName("genres")]
        public Genre[]? Genres { get; set; }

        [JsonPropertyName("homepage")]
        public string? Homepage { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("imdb_id")]
        public string? ImdbId { get; set; }

        [JsonPropertyName("original_language")]
        public string? OriginalLanguage { get; set; }

        [JsonPropertyName("original_title")]
        public string? OriginalTitle { get; set; }

        [JsonPropertyName("overview")]
        public string? Overview { get; set; }

        [JsonPropertyName("popularity")]
        public float Popularity { get; set; }

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }

        [JsonPropertyName("production_companies")]
        public ProductionCompany[]? ProductionCompanies { get; set; }

        [JsonPropertyName("production_countries")]
        public ProductionCountry[]? ProductionCountries { get; set; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("revenue")]
        public int Revenue { get; set; }

        [JsonPropertyName("runtime")]
        public int Runtime { get; set; }

        [JsonPropertyName("spoken_languages")]
        public SpokenLanguage[]? SpokenLanguages { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("tagline")]
        public string? Tagline { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("video")]
        public bool Video { get; set; }

        [JsonPropertyName("vote_average")]
        public float VoteAverage { get; set; }

        [JsonPropertyName("vote_count")]
        public float VoteCount { get; set; }

        [JsonPropertyName("credits")]
        public Credits? Credits { get; set; }

        [JsonPropertyName("images")]
        public Images? Images { get; set; }

        [JsonPropertyName("videos")]
        public Videos? Videos { get; set; }

        [JsonPropertyName("release_dates")]
        public ReleaseDates? ReleaseDates { get; set; }
    }

    public class BelongsToCollection
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }

        [JsonPropertyName("backdrop_path")]
        public string? BackdropPath { get; set; }
    }

    public class Credits
    {
        [JsonPropertyName("cast")]
        public Cast[]? Cast { get; set; }

        [JsonPropertyName("crew")]
        public Cast[]? Crew { get; set; }
    }

    public class Cast
    {
        [JsonPropertyName("adult")]
        public bool Adult { get; set; }

        [JsonPropertyName("gender")]
        public int Gender { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("known_for_department")]
        public string? KnownForDepartment { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("original_name")]
        public string? OriginalName { get; set; }

        [JsonPropertyName("popularity")]
        public float Popularity { get; set; }

        [JsonPropertyName("profile_path")]
        public string? ProfilePath { get; set; }

        [JsonPropertyName("cast_id")]
        public int? CastId { get; set; }

        [JsonPropertyName("character")]
        public string? Character { get; set; }

        [JsonPropertyName("credit_id")]
        public string? CreditId { get; set; }

        [JsonPropertyName("order")]
        public float? Order { get; set; }

        [JsonPropertyName("department")]
        public string? Department { get; set; }

        [JsonPropertyName("job")]
        public string? Job { get; set; }
    }

    public class Genre
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class Images
    {
        [JsonPropertyName("backdrops")]
        public object[]? Backdrops { get; set; }

        [JsonPropertyName("logos")]
        public object[]? Logos { get; set; }

        [JsonPropertyName("posters")]
        public object[]? Posters { get; set; }
    }

    public class ProductionCompany
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("logo_path")]
        public string? LogoPath { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("origin_country")]
        public string? OriginCountry { get; set; }
    }

    public class ProductionCountry
    {
        [JsonPropertyName("iso_3166_1")]
        public string? Iso3166_1 { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class ReleaseDates
    {
        [JsonPropertyName("results")]
        public ReleaseDatesResult[]? Results { get; set; }
    }

    public class ReleaseDatesResult
    {
        [JsonPropertyName("iso_3166_1")]
        public string? Iso3166_1 { get; set; }

        [JsonPropertyName("release_dates")]
        public ReleaseDate[]? ReleaseDates { get; set; }
    }

    public class ReleaseDate
    {
        [JsonPropertyName("certification")]
        public string? Certification { get; set; }

        [JsonPropertyName("descriptors")]
        public object[]? Descriptors { get; set; }

        [JsonPropertyName("iso_639_1")]
        public string? Iso639_1 { get; set; }

        [JsonPropertyName("note")]
        public string? Note { get; set; }

        [JsonPropertyName("release_date")]
        public string? Release_Date { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }
    }

    public class SpokenLanguage
    {
        [JsonPropertyName("english_name")]
        public string? EnglishName { get; set; }

        [JsonPropertyName("iso_639_1")]
        public string? Iso639_1 { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class Videos
    {
        [JsonPropertyName("results")]
        public VideoResult[]? Results { get; set; }
    }

    public partial class VideoResult
    {
        [JsonPropertyName("iso_639_1")]
        public string? Iso639_1 { get; set; }

        [JsonPropertyName("iso_3166_1")]
        public string? Iso3166_1 { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("site")]
        public string? Site { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("official")]
        public bool Official { get; set; }

        [JsonPropertyName("published_at")]
        public string? PublishedAt { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }
}
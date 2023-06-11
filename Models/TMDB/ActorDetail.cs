namespace WhatMovie.Models.TMDB
{
    public class ActorDetail
    {
        public bool Adult { get; set; }
        public string[]? AlsoKnownAs { get; set; }
        public string? Biography { get; set; }
        public string? Birthday { get; set; }
        public object? Deathday { get; set; }
        public int Gender { get; set; }
        public object? Homepage { get; set; }
        public int Id { get; set; }
        public string? ImdbId { get; set; }
        public string? KnownForDepartment { get; set; }
        public string? Name { get; set; }
        public string? PlaceOfBirth { get; set; }
        public float Popularity { get; set; }
        public string? ProfilePath { get; set; }
    }
}
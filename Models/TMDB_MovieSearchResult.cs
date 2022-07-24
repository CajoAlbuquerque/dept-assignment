﻿using System.ComponentModel.DataAnnotations;

namespace MovieTrailersAPI.Models
{
    public class TMDB_MovieSearchResult
    {
        [Key]
        public string movie_id {get; set;}
        public string title {get; set;}
        public string original_title {get; set;}
        public string original_language {get; set;}
        public string overview {get; set;}
        public string release_date {get; set;}
        public Boolean adult {get; set;}
        public IEnumerable<int> genre_ids {get; set;}
        public string? backdrop_path {get; set;}
        public string? poster_path {get; set;}
        public int vote_count {get; set;}
        public float vote_average {get; set;}
        public float popularity {get; set;}
        public Boolean video {get; set;}
    }
}
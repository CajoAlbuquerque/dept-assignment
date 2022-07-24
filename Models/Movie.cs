using System.ComponentModel.DataAnnotations;

namespace MovieTrailersAPI.Models
{
    public class Movie
    {
        [Key]
        public int movie_id { get; set; }
        public string? name { get; set; }
        public string? language { get; set; }

        [Required]
        public IList<Trailer> trailers { get; set; }

        public Movie(int movie_id, string name, string language, IList<Trailer> trailers)
        {
            this.movie_id = movie_id;
            this.name = name;
            this.language = language;
            this.trailers = trailers;
        }

        public Movie(int movie_id, string name, string language) : this(movie_id, name, language, new List<Trailer>()) { }
        public Movie(int movie_id) : this(movie_id, "", "", new List<Trailer>()) { }
    }
}

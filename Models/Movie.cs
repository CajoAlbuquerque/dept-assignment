using System.ComponentModel.DataAnnotations;

namespace MovieTrailersAPI.Models
{
    public class Movie
    {
        [Key]
        public int movie_id { get; set; }
        [Required]
        public IList<Trailer> trailers { get; set; }

        public Movie(int movie_id, IList<Trailer> trailers)
        {
            this.movie_id = movie_id;
            this.trailers = trailers;
        }

        public Movie(int movie_id) : this(movie_id, new List<Trailer>()) { }
    }
}

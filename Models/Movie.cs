using MovieTrailersAPI.Models.TMDB;

namespace MovieTrailersAPI.Models
{
    public class Movie
    {
        public int movie_id { get; set; }
        public string title { get; set; }
        public string language { get; set; }
        public string? original_title { get; set; }
        public string? overview { get; set; }
        public string? release_date { get; set; }
        public bool? adult { get; set; }
        public IList<Trailer> trailers { get; set; }

        public Movie(int movie_id, string title, string language, IList<Trailer> trailers)
        {
            this.movie_id = movie_id;
            this.title = title;
            this.language = language;
            this.trailers = trailers;
        }

        public Movie(TMDB_MovieSearchResult movie, IList<Trailer> trailers)
        {
            this.movie_id = movie.id;
            this.title = movie.title;
            this.original_title = movie.original_title;
            this.language = movie.original_language;
            this.overview = movie.overview;
            this.release_date = movie.release_date;
            this.adult = movie.adult;
            this.trailers = trailers;
        }

        public Movie(TMDB_MovieSearchResult movie) : this(movie, new List<Trailer>()) { }
    }
}

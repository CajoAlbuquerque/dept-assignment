using MovieTrailersAPI.Models;

namespace MovieTrailersAPI.Providers
{
    public interface IMovieProvider
    {
        public Task<HttpResponseMessage> GetMovies(string query);
        public Task<TMDB_Search> ProcessMoviesResponse(HttpResponseMessage response);
    }
}

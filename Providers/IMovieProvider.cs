using MovieTrailersAPI.Models.TMDB;

namespace MovieTrailersAPI.Providers
{
    public interface IMovieProvider
    {
        public Task<HttpResponseMessage> GetMovies(string query, int page);
        public Task<TMDB_Search> ProcessMoviesResponse(HttpResponseMessage response);
        public Task<HttpResponseMessage> GetTrailers(int movieId);
        public Task<IEnumerable<TMDB_Video>> ProcessTrailersResponse(HttpResponseMessage response);

    }
}

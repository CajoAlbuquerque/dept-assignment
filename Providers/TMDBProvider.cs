using MovieTrailersAPI.Models;
using System.Text.Json;

namespace MovieTrailersAPI.Providers
{
    public class TMDBProvider : IMovieProvider
    {
        private string searchUrl { get; }
        private string videosUrl { get; }
        private string apiKey { get; }

        public HttpClient httpClient { get; }

        public TMDBProvider(IConfiguration config, HttpClient client)
        {
            httpClient = client;
            apiKey = config["TMDB:ApiKey"];
            searchUrl = "https://api.themoviedb.org/3/search/movie?api_key=" + apiKey + "&query={0}";
            videosUrl = "https://api.themoviedb.org/3/movie/{0}/videos?api_key=" + apiKey;
        }

        public async Task<HttpResponseMessage> GetMovies(string query)
        {
            var endpoint = String.Format(searchUrl, query);
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            return await httpClient.SendAsync(request);
        }

        public async Task<TMDB_Search> ProcessMoviesResponse(HttpResponseMessage response)
        {
            TMDB_Search? searchResult = await JsonSerializer.DeserializeAsync<TMDB_Search>(await response.Content.ReadAsStreamAsync());

            if (searchResult != null)
            {
                searchResult.results = searchResult.results.OrderByDescending(movie => movie.popularity);
            }

            return searchResult;
        }
    }
}

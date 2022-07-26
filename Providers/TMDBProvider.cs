﻿using MovieTrailersAPI.Models.TMDB;
using System.Text.Json;

namespace MovieTrailersAPI.Providers
{
    public class TMDBProvider : IMovieProvider
    {
        private string searchUrl { get; }
        private string videosUrl { get; }
        private string movieUrl { get; }
        private string apiKey { get; }

        public HttpClient httpClient { get; }

        public TMDBProvider(IWebHostEnvironment environment, IConfiguration config, HttpClient client)
        {
            if (environment.IsDevelopment()){
                apiKey = config["TMDB:ApiKey"];
            }
            else{
                Console.WriteLine(Environment.GetEnvironmentVariable("API_KEY"));
                apiKey = Environment.GetEnvironmentVariable("API_KEY");
            }

            httpClient = client;
            searchUrl = "https://api.themoviedb.org/3/search/movie?api_key=" + apiKey + "&query={0}&page={1}";
            videosUrl = "https://api.themoviedb.org/3/movie/{0}/videos?api_key=" + apiKey;
            movieUrl = "https://api.themoviedb.org/3/movie/{0}?api_key=" + apiKey;
        }

        public async Task<HttpResponseMessage> GetMovies(string query, int page)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, String.Format(searchUrl, query, page));
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

        public async Task<HttpResponseMessage> GetTrailers(int movieId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, String.Format(videosUrl, movieId));
            return await httpClient.SendAsync(request);
        }

        public async Task<IEnumerable<TMDB_Video>> ProcessTrailersResponse(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            TMDB_MovieVideo? videoResults = await JsonSerializer.DeserializeAsync<TMDB_MovieVideo>(await response.Content.ReadAsStreamAsync());

            if (videoResults == null)
            {
                Array.Empty<TMDB_Video>();
            }

            return videoResults.results.Where(video => video.type.Equals("trailer", StringComparison.OrdinalIgnoreCase));
        }

        public async Task<HttpResponseMessage> GetMovieDetails(int movieId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, String.Format(movieUrl, movieId));
            return await httpClient.SendAsync(request);
        }

        public async Task<TMDB_MovieSearchResult> ProcessMovieDetailsResponse(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            return await JsonSerializer.DeserializeAsync<TMDB_MovieSearchResult>(await response.Content.ReadAsStreamAsync());
        }
    }
}

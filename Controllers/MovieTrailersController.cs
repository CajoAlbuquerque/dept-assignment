using Microsoft.AspNetCore.Mvc;
using MovieTrailersAPI.Models;
using MovieTrailersAPI.Providers;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieTrailersAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class MovieTrailersController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMovieProvider _provider;


        public MovieTrailersController(IHttpClientFactory httpClientFactory, IConfiguration config, IMovieProvider provider)
        {
            _httpClientFactory = httpClientFactory;
            _provider = provider;
        }


        // GET api/search?query="Avengers"
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<TMDB_Search>> Get([FromQuery] string query)
        {
            // TODO: check the input or filter it
            var response = await _provider.GetMovies(query);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response.Content.ReadAsStringAsync());
            }

            var searchResult = await _provider.ProcessMoviesResponse(response);

            return Ok(searchResult);
        }

        // GET api/trailers/{id}
        [HttpGet]
        [Route("trailers/{id}")]
        public async Task<ActionResult<IEnumerable<TMDB_Video>>> GetTrailers(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.themoviedb.org/3/movie/{id}/videos?api_key=");

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response.Content.ReadAsStringAsync());
            }

            TMDB_MovieVideo? videoResults = await JsonSerializer.DeserializeAsync<TMDB_MovieVideo>(await response.Content.ReadAsStreamAsync());

            if (videoResults == null)
            {
                return Ok(Array.Empty<TMDB_Video>());
            }

            var trailerResults = videoResults.results.Where(video => video.type.Equals("trailer", StringComparison.OrdinalIgnoreCase));
            return Ok(trailerResults);
        }

        // GET api/trailers/
        [HttpGet]
        [Route("trailers/")]
        public async Task<ActionResult<IEnumerable<TMDB_Video>>> GetTrailers([FromBody] string query)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.themoviedb.org/3/search/movie?api_key=&query={query}");

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(request);

            return Ok(response.Content.ReadAsStreamAsync());
        }
    }
}

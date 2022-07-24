using Microsoft.AspNetCore.Mvc;
using MovieTrailersAPI.Models;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieTrailersAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class MovieTrailersController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;


        public MovieTrailersController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = config["TMDB:ApiKey"];
        }


        // GET api/search?query="Avengers"
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<TMDB_Search>> Get([FromQuery] string query)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.themoviedb.org/3/search/movie?api_key={_apiKey}&query={query}");

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response.Content.ReadAsStringAsync());
            }

            TMDB_Search? searchResult = await JsonSerializer.DeserializeAsync<TMDB_Search>(await response.Content.ReadAsStreamAsync());

            if (searchResult != null)
            {
                searchResult.results = searchResult.results.OrderByDescending(movie => movie.popularity);
            }

            return Ok(searchResult);
        }

        // GET api/trailers/{id}
        [HttpGet]
        [Route("trailers/{id}")]
        public async Task<ActionResult<IEnumerable<TMDB_Video>>> GetTrailers(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.themoviedb.org/3/movie/{id}/videos?api_key={_apiKey}");

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

        // PUT api/search/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/search/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

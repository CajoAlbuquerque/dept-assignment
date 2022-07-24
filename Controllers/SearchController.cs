using Microsoft.AspNetCore.Mvc;
using MovieTrailersAPI.Models;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieTrailersAPI.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;


        public SearchController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = config["TMDB:ApiKey"];
        }


        // GET api/search
        [HttpGet]
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

            return Ok(searchResult);
        }

        // POST api/search
        [HttpPost]
        public void Post([FromBody] string value)
        {
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

﻿using Microsoft.AspNetCore.Mvc;
using MovieTrailersAPI.Models;
using MovieTrailersAPI.Models.TMDB;
using MovieTrailersAPI.Providers;

namespace MovieTrailersAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class MovieTrailersController : ControllerBase
    {
        private readonly IMovieProvider _provider;

        public MovieTrailersController(IMovieProvider provider)
        {
            _provider = provider;
        }

        // GET api/search?query=Avengers&page=1
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<TMDB_Search>> Get([FromQuery] string query, [FromQuery] int page = 1)
        {
            var response = await _provider.GetMovies(query, page);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response.Content.ReadAsStringAsync());
            }

            return Ok(await _provider.ProcessMoviesResponse(response));
        }

        // GET api/movies/{id}/trailers
        [HttpGet]
        [Route("movies/{id}/trailers")]
        public async Task<ActionResult<IEnumerable<TMDB_Video>>> GetTrailers(int id)
        {
            var response = await _provider.GetTrailers(id);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response.Content.ReadAsStringAsync());
            }
            
            return Ok(await _provider.ProcessTrailersResponse(response));
        }

        // GET api/movies/{id}
        [HttpGet]
        [Route("movies/{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var trailersResponse = await _provider.GetTrailers(id);
            var movieResponse = await _provider.GetMovieDetails(id);

            if (!trailersResponse.IsSuccessStatusCode)
            {
                return BadRequest(trailersResponse.Content.ReadAsStringAsync());
            }
            if (!movieResponse.IsSuccessStatusCode)
            {
                return BadRequest(movieResponse.Content.ReadAsStringAsync());
            }
            
            var taskMovie = _provider.ProcessMovieDetailsResponse(movieResponse);
            var taskTrailers = _provider.ProcessTrailersResponse(trailersResponse);

            taskMovie.Wait();
            taskTrailers.Wait();

            var movieDetails = taskMovie.Result;
            var trailers = taskTrailers.Result.Select(trailer => new Trailer(trailer));

            return Ok(new Movie(movieDetails, trailers.ToList()));
        }

        // GET api/movies/trailers?query=Avengers&page=1
        // [HttpGet]
        // [Route("movies/trailers-sync")]
        // public async Task<ActionResult<IEnumerable<TMDB_Video>>> GetTrailers([FromQuery] string query, [FromQuery] int page = 1)
        // {
        //     var finalResponse = new MovieTrailersResponse(page);
        //     var moviesResponse = await _provider.GetMovies(query, page);

        //     if (!moviesResponse.IsSuccessStatusCode)
        //     {
        //         return BadRequest(moviesResponse.Content.ReadAsStringAsync());
        //     }

        //     var moviesResult = await _provider.ProcessMoviesResponse(moviesResponse);

        //     if (moviesResult == null)
        //     {
        //         return BadRequest("No movies found for the provided query");
        //     }

        //     foreach (var movie in moviesResult.results)
        //     {
        //         var movieEntry = new Movie(movie);
        //         var trailersResponse = await _provider.GetTrailers(movie.id);

        //         if (!trailersResponse.IsSuccessStatusCode)
        //         {
        //             return BadRequest(trailersResponse.Content.ReadAsStringAsync());
        //         }

        //         var trailersResult = await _provider.ProcessTrailersResponse(trailersResponse);

        //         if (trailersResult != null)
        //         {
        //             foreach (var trailer in trailersResult)
        //             {
        //                 movieEntry.trailers.Add(new Trailer(trailer));
        //             }
        //         }

        //         finalResponse.results.Add(movieEntry);
        //     }

        //     return Ok(finalResponse);
        // }

        // GET api/movies/trailers?query=Avengers&page=1
        [HttpGet]
        [Route("movies/trailers")]
        public async Task<ActionResult<MovieTrailersResponse>> GetTrailersAsync([FromQuery] string query, [FromQuery] int page = 1)
        {
            var finalResponse = new MovieTrailersResponse(page);
            var moviesResponse = await _provider.GetMovies(query, page);

            if (!moviesResponse.IsSuccessStatusCode)
            {
                return BadRequest(moviesResponse.Content.ReadAsStringAsync());
            }

            var moviesResult = await _provider.ProcessMoviesResponse(moviesResponse);

            if (moviesResult == null)
            {
                return BadRequest("No movies found for the provided query");
            }

            finalResponse.total_pages = moviesResult.total_pages;

            var tasks = moviesResult.results.Select(movie =>
            {
                var movieEntry = new Movie(movie);
                return _provider.GetTrailers(movie.id).ContinueWith(async (task) =>
                {
                    var trailersResponse = task.Result;

                    if (!trailersResponse.IsSuccessStatusCode) return;

                    var trailersResult = await _provider.ProcessTrailersResponse(trailersResponse);

                    if (trailersResult != null)
                    {
                        foreach (var trailer in trailersResult)
                        {
                            movieEntry.trailers.Add(new Trailer(trailer));
                        }
                    }

                    finalResponse.results.Add(movieEntry);
                });
            });

            await Task.WhenAll(tasks);
            return Ok(finalResponse);
        }
    }
}

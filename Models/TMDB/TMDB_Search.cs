using System.ComponentModel.DataAnnotations;

namespace MovieTrailersAPI.Models.TMDB
{
    public class TMDB_Search
    {
        public int page { get; set; }
        public IEnumerable<TMDB_MovieSearchResult> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }
}

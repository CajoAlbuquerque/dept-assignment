using System.ComponentModel.DataAnnotations;

namespace MovieTrailersAPI.Models.TMDB
{
    public class TMDB_MovieVideo
    {
        [Key]
        public int id { get; set; }
        public IEnumerable<TMDB_Video> results { get; set; }
    }
}

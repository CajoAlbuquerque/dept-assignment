using System.ComponentModel.DataAnnotations;

namespace MovieTrailersAPI.Models
{
    public class TMDB_MovieVideo
    {
        [Key]
        public string movie_id { get; set; }
        public IEnumerable<TMDB_Video> videos { get; set; }
    }
}

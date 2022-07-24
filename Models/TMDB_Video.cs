using System.ComponentModel.DataAnnotations;

namespace MovieTrailersAPI.Models
{
    public class TMDB_Video
    {
        [Key]
        public string video_id { get; set; }
        public string iso_639_1 { get; set; }
        public string iso_3166_1 { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string site { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public Boolean official { get; set; }
        public string published_at { get; set; }
    }
}

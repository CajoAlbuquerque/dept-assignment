namespace MovieTrailersAPI.Models.TMDB
{
    public class TMDB_MovieVideo
    {
        public int id { get; set; }
        public IEnumerable<TMDB_Video> results { get; set; }
    }
}

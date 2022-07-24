namespace MovieTrailersAPI.Models
{
    public class MovieTrailersResponse
    {
        public int page { get; set; }
        public int total_pages { get; set; }
        public IList<Movie> results { get; set; }

        public MovieTrailersResponse(int page, int total_pages, IList<Movie> results)
        {
            this.page = page;
            this.total_pages = total_pages;
            this.results = results;
        }

        public MovieTrailersResponse(int page) : this(page, page, new List<Movie>()) { }
    }
}

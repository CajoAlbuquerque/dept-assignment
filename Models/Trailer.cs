using MovieTrailersAPI.Models.TMDB;
using System.ComponentModel.DataAnnotations;

namespace MovieTrailersAPI.Models
{
    public class Trailer
    {
        [Key]
        public string id { get; set; }
        [Required]
        public string? url { get; set; }
        public string? name { get; set; }
        public string? language { get; set; }
        public bool? official { get; set; }
        public string? published_at { get; set; }

        public Trailer(string id, string url, string name, string language, bool official, string published_at)
        {
            this.id = id;
            this.url = url;
            this.name = name;
            this.language = language;
            this.official = official;
            this.published_at = published_at;
        }

        public Trailer(string id, string url) : this(id, url, null, null, false, null) { }

        public Trailer(TMDB_Video video) //TODO: replace by an interface
        {
            this.id = video.id;
            this.url = VideoUrlFactory.GetUrl(video.site, video.key);
            this.name = video.name;
            this.language = $"{video.iso_639_1}-{video.iso_3166_1}";
            this.official = video.official;
            this.published_at = video.published_at;
        }

        private static class VideoUrlFactory
        {
            private static string youtubeUrl = "https://www.youtube.com/watch?v={0}";
            private static string vimeoUrl = "https://vimeo.com/{0}";

            public static string GetUrl(string site, string key) {
                string siteUrl;
                
                switch (site)
                {
                    case "Youtube":
                        siteUrl = youtubeUrl;
                        break;

                    case "Vimeo":
                        siteUrl = vimeoUrl;
                        break;

                    default:
                        siteUrl = youtubeUrl;
                        break;
                }

                return String.Format(siteUrl, key);
            }
        }
    }
}

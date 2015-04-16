using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PlexHelper.Yts.Entities
{
    public class YtsMovieListResponse
    {
        [JsonProperty("movie_count")]
        public int MovieCount { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("page_number")]
        public int PageNumber { get; set; }

        [JsonProperty("movies")]
        public List<YtsMovie> Movies { get; set; } 

        public YtsMovieListResponse()
        {
            Movies = new List<YtsMovie>();
        }

        public override string ToString()
        {
            return string.Format("Movie Count: {0}, Limit: {1}, Page Number: {2}", MovieCount, Limit, PageNumber);
        }
    }
}

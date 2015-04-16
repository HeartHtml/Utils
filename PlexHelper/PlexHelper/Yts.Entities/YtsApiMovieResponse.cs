using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlexHelper.Yts.Entities
{
    public class YtsApiMovieResponse : YtsApiResponse
    {
        [JsonProperty("data")]
        public YtsMovieListResponse MovieListResponse { get; set; }

        public YtsApiMovieResponse()
        {
            MovieListResponse = new YtsMovieListResponse();
        }

        public YtsApiMovieResponse(string json)
        {
            YtsApiMovieResponse movieResponse = JsonConvert.DeserializeObject<YtsApiMovieResponse>(json);

            MovieListResponse = movieResponse.MovieListResponse;
        }

    }
}

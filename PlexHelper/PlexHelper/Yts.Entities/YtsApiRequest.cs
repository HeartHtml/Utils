using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PlexHelper.Yts.Entities
{
    public class YtsApiRequest
    {
        protected string MovieSearchUrl
        {
            get
            {
                return Properties.Settings.Default.MovieSearchUrl;
            }
        }

        public async Task<YtsApiMovieResponse> MovieListQueryAsync(string movieNameQuery)
        {
            using (var client = new HttpClient())
            {
                var responseString = await client.GetStringAsync(MovieSearchUrl);

                YtsApiMovieResponse listResponse = new YtsApiMovieResponse(responseString);

                return listResponse;
            }
        }
    }
}

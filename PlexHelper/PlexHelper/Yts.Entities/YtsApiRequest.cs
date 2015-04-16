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

        public async Task<YtsApiMovieResponse> MovieListQueryAsync(string movieNameQuery, bool exhaustiveSearch = false)
        {
            YtsApiMovieResponse listResponse = new YtsApiMovieResponse();

            bool continueSearch;

            int page = 0;

            do
            {
                using (var client = new HttpClient())
                {
                    page++;

                    var responseString = await client.GetStringAsync(string.Format(MovieSearchUrl, movieNameQuery, page));

                    YtsApiMovieResponse innerResponse = new YtsApiMovieResponse(responseString);

                    listResponse.MovieListResponse.Limit = innerResponse.MovieListResponse.Limit;

                    listResponse.MovieListResponse.MovieCount = innerResponse.MovieListResponse.MovieCount;

                    listResponse.MovieListResponse.PageNumber = innerResponse.MovieListResponse.PageNumber;

                    if (innerResponse.MovieListResponse.Movies.Count == 0)
                    {
                        break;
                    }

                    listResponse.MovieListResponse.Movies.AddRange(innerResponse.MovieListResponse.Movies);

                    YtsMovie movieFound =
                        listResponse.MovieListResponse.Movies.FirstOrDefault(dd => movieNameQuery.Contains(dd.Title));

                    continueSearch = movieFound == null;
                }
            } while (exhaustiveSearch && continueSearch);

            return listResponse;
        }
    }
}

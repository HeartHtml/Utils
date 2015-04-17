using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UtilsLib.ExtensionMethods;

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

                    //We found the movie, no need to search anymore
                    if (listResponse.MovieListResponse.Movies.SafeAny(
                            dd => FormatterHelper.GetFormattedMovieName(dd).SafeEquals(movieNameQuery)))
                    {
                        break;
                    }
                }
            } while (exhaustiveSearch);

            foreach (YtsMovie movie in listResponse.MovieListResponse.Movies)
            {
                movie.FuzzyDistance = StringExtensions.ComputeLevenshteinDistance(movieNameQuery, movie.Title);
            }

            return listResponse;
        }
    }
}

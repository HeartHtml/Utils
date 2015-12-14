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

        public async Task<YtsApiMovieResponse> MovieListQueryAsync(string movieNameQuery, int year, bool exhaustiveSearch = false)
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

                    string formattedMovieQuery = FormatterHelper.FormatMovie(movieNameQuery, year);

                    //We found the movie, no need to search anymore
                    if (listResponse.MovieListResponse.Movies.SafeAny(dd => FormatterHelper.FormatMovie(dd.Title, dd.Year).SafeEquals(formattedMovieQuery)))
                    {
                        break;
                    }
                }
            } while (exhaustiveSearch);

            foreach (YtsMovie movie in listResponse.MovieListResponse.Movies)
            {
                string compareMovieQuery = FormatterHelper.FormatMovie(movieNameQuery, year);

                string foundMovieTitle = FormatterHelper.FormatMovie(movie.Title, movie.Year);

                movie.FuzzyDistance = StringExtensions.ComputeLevenshteinDistance(compareMovieQuery, foundMovieTitle);
            }

            return listResponse;
        }
    }
}

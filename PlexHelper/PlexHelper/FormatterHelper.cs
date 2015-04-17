using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PlexHelper.Yts.Entities;
using UtilsLib.ExtensionMethods;

namespace PlexHelper
{
    public class FormatterHelper
    {
        protected static string DownloadedMovieNameFormat
        {
            get
            {
                return Properties.Settings.Default.DownloadedMovieNameFormat;
            }
        }

        protected static string[] StringsToReplace
        {
            get
            {
                return Properties.Settings.Default.StringsToReplace.Split("|");
            }
        }

        static readonly List<int> Repository = new List<int> ();

        private static List<int> YearRepository
        {
            get
            {
                if (!Repository.SafeAny())
                {
                    const int min = 1850;

                    const int max = 2999;

                    for (int i = min; i < max; i ++)
                    {
                        Repository.Add(i);
                    }
                }

                return Repository;
            }
        }

        /// <summary>
        /// Formats the name of a YtsMovie entity using more concrete rules
        /// </summary>
        /// <param name="movie"></param>
        /// <returns>A string representing the formatted name of the movie depending on the rules</returns>
        public static string GetFormattedMovieName(YtsMovie movie)
        {
            PropertyInfo[] collection = movie.GetType().GetProperties();

            string movieName = DownloadedMovieNameFormat;

            foreach (PropertyInfo propertyInfo in collection)
            {
                if (DownloadedMovieNameFormat.Contains(propertyInfo.Name))
                {
                    movieName = movieName.Replace(propertyInfo.Name, propertyInfo.GetValue(movie, null).ToString());
                }
            }

            return movieName;
        }

        /// <summary>
        /// Formats the original file name according with a basic formatting
        /// </summary>
        /// <param name="originalFileName"></param>
        /// <returns>A string representing the formatted name of the movie depending on the rules</returns>
        public static string GetFormattedMovieName(string originalFileName)
        {
            string cleanedFile = originalFileName;
            
            foreach (string candidate in StringsToReplace)
            {
                cleanedFile = cleanedFile.Replace(candidate, " ");
            }

            cleanedFile = cleanedFile.TrimSafely();

            string[] pieces = cleanedFile.Split(" ");

            int potentialYear;

            if (int.TryParse(pieces[pieces.Length - 1], out potentialYear))
            {
                if (YearRepository.Contains(potentialYear))
                {
                    cleanedFile = cleanedFile.Replace(potentialYear.ToString(CultureInfo.InvariantCulture), string.Empty).TrimSafely();

                    cleanedFile = string.Format("{0} ({1})", cleanedFile, potentialYear);
                }
            }

            return cleanedFile;
        }
    }
}

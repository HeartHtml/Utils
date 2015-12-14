using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlexHelper.Yts.Entities;

namespace PlexHelper.Test
{
    [TestClass]
    public class FormatterHelperTest
    {
        [TestMethod]
        public void YtsMovieTest()
        {
            YtsMovie movie = new YtsMovie { Title = "Titanic", Year = 1997 };

            string movieName = FormatterHelper.FormatMovie(movie.Title, movie.Year);

            Assert.AreEqual("Titanic (1997)", movieName);
        }

        [TestMethod]
        public void RenameRawTest()
        {
            const string rawName = "The.Fast.and.the.Furious.2001.1080p.BrRip.x264.YIFY+HI";

            string name = FormatterHelper.GetFormattedMovieName(rawName);

            Assert.AreEqual("The Fast and the Furious (2001)", name);
        }

    }
}

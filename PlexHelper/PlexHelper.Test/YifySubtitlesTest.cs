using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlexHelper.YifySubtitles.Entities;

namespace PlexHelper.Test
{
    [TestClass]
    public class YifySubtitlesTest
    {
        [TestMethod]
        public void SearchTest()
        {
            SubtitleDownloader downloader = new SubtitleDownloader();

            SubtitleDataFileCollection array = downloader.DownloadSubtitlesAsync("tt1245492").Result;

            Assert.IsNotNull(array);
        }
    }
}

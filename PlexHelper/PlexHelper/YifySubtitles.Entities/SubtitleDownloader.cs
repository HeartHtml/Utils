using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using UtilsLib.ExtensionMethods;

namespace PlexHelper.YifySubtitles.Entities
{
    public class SubtitleDownloader
    {
        protected string SubtitleSearchUrl
        {
            get
            {
                return Properties.Settings.Default.SubtitleSearchUrl;
            }
        }

        protected string SubtitleDownloadFormatLink
        {
            get
            {
                return Properties.Settings.Default.SubtitleDownloadFormatLink;
            }
        }

        protected string[] SubtitleLanguages
        {
            get
            {
                return Properties.Settings.Default.SubtitleLanguages.Split("|");
            }
        }

        public async Task<SubtitleDataFileCollection> DownloadSubtitlesAsync(string imdbCode)
        {
            SubtitleDataFileCollection dataFileCollection = null;

            string finalUrl = string.Format(SubtitleSearchUrl, imdbCode);

            using (var client = new HttpClient())
            {
                var responseString = await client.GetStringAsync(finalUrl);

                HtmlDocument doc = new HtmlDocument();

                doc.LoadHtml(responseString);

                //Retrieve all unordered lists in document
                List<HtmlNode> unorderedLists = doc.DocumentNode.SelectNodes("//ul").ToList();

                List<Subtitle> subtitlesFound = new List<Subtitle>();

                foreach (HtmlNode unorderedListNodes in unorderedLists)
                {
                    if (unorderedListNodes.HasAttributes)
                    {
                        HtmlAttribute attribute = unorderedListNodes.Attributes["class"];

                        //Subs are found, they are always in the unordered list with this class with an li for each subtitle
                        if (attribute.Value.SafeEquals("other-subs"))
                        {
                            HtmlNodeCollection liNodes = unorderedListNodes.ChildNodes;

                            foreach (HtmlNode liNode in liNodes)
                            {
                                Subtitle subtitle = new Subtitle();

                                if (liNode.HasAttributes)
                                {
                                    HtmlAttribute dataIdAttribute = liNode.Attributes["data-id"];

                                    if (dataIdAttribute != null)
                                    {
                                        subtitle.DataId = Convert.ToInt32(dataIdAttribute.Value);
                                    }
                                }

                                //Loop through each li

                                HtmlNodeCollection liChildNodes = liNode.ChildNodes;

                                foreach (HtmlNode childNode in liChildNodes)
                                {
                                    if (childNode.HasAttributes)
                                    {
                                        HtmlAttribute childNodeAttribute = childNode.Attributes["class"];

                                        //Find download link
                                        if (childNodeAttribute.Value.SafeEquals("subtitle-download"))
                                        {
                                            HtmlAttribute hrefAttribute = childNode.Attributes["href"];

                                            if (hrefAttribute != null)
                                            {
                                                string fileName = hrefAttribute.Value.Replace("/subtitles/",
                                                    string.Empty);

                                                subtitle.FileName = fileName;

                                                subtitle.DownloadUrl = string.Format(SubtitleDownloadFormatLink,
                                                    fileName);
                                            }
                                        }

                                        //Find rating
                                        if (childNodeAttribute.Value.SafeEquals("rating"))
                                        {
                                            foreach (HtmlNode ratingNode in childNode.ChildNodes)
                                            {
                                                if (ratingNode.HasAttributes)
                                                {
                                                    HtmlAttribute ratingNodeAttribute = ratingNode.Attributes["class"];

                                                    if (ratingNodeAttribute != null &&
                                                        ratingNodeAttribute.Value.SafeEquals("reply-rating"))
                                                    {
                                                        subtitle.Rating = Convert.ToInt32(ratingNode.InnerText);
                                                    }
                                                }
                                            }
                                        }

                                        //Find language
                                        if (childNodeAttribute.Value.SafeEquals("subtitle-page"))
                                        {
                                            foreach (HtmlNode subtitlePageNode in childNode.ChildNodes)
                                            {
                                                if (SubtitleLanguages.Contains(subtitlePageNode.InnerText))
                                                {
                                                    subtitle.Language = subtitlePageNode.InnerText;
                                                }
                                            }
                                        }
                                    }
                                }

                                subtitlesFound.Add(subtitle);
                            }
                        }
                    }
                }

                if (subtitlesFound.SafeAny())
                {
                    List<Subtitle> subtitlesToDownload = new List<Subtitle>();

                    foreach (Subtitle subtitle in subtitlesFound)
                    {
                        Subtitle subtitle1 = subtitle;
                        //If the list already contains a file with that language
                        //Overwrite it if the rating is higher
                        if (subtitlesToDownload.SafeAny(dd => dd.Language.SafeEquals(subtitle1.Language)))
                        {
                            Subtitle subtitleInList =
                                subtitlesToDownload.FirstOrDefault(dd => dd.Language.SafeEquals(subtitle1.Language));

                            if (subtitleInList != null &&
                                subtitle1.Rating > subtitleInList.Rating)
                            {
                                subtitleInList.DataId = subtitle1.DataId;

                                subtitleInList.DownloadUrl = subtitle1.DownloadUrl;

                                subtitleInList.Language = subtitle1.Language;

                                subtitleInList.Rating = subtitle1.Rating;
                            }
                        }
                        else
                        {
                            subtitlesToDownload.Add(subtitle);
                        }
                    }

                    dataFileCollection = new SubtitleDataFileCollection();

                    foreach (Subtitle subtitle in subtitlesToDownload.Where(dd => SubtitleLanguages.Contains(dd.Language)))
                    {
                        var data = await client.GetByteArrayAsync(subtitle.DownloadUrl);

                        SubtitleDataFile dataFile = new SubtitleDataFile
                        {
                            Data = data,
                            Extension = ".zip",
                            FileName = subtitle.FileName
                        };

                        dataFileCollection.DataFiles.Add(dataFile);
                    }
                }

                return dataFileCollection;
            }
        }
    }
}

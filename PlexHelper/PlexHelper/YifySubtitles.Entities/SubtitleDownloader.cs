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
                List<HtmlNode> allTables = doc.DocumentNode.SelectNodes("//table").ToList();

                List<Subtitle> subtitlesFound = new List<Subtitle>();

                foreach (HtmlNode tableNode in allTables)
                {
                    if (tableNode.HasAttributes)
                    {
                        HtmlAttribute attribute = tableNode.Attributes["class"];

                        //Subs are found, they are always in the unordered list with this class with an li for each subtitle
                        if (attribute.Value.Contains("other-subs"))
                        {
                            HtmlNodeCollection tableRowNodes = tableNode.SelectNodes("//tbody/tr");

                            foreach (HtmlNode tableRow in tableRowNodes)
                            {
                                Subtitle subtitle = new Subtitle();

                                if (tableRow.HasAttributes)
                                {
                                    HtmlAttribute dataIdAttribute = tableRow.Attributes["data-id"];

                                    if (dataIdAttribute != null)
                                    {
                                        subtitle.DataId = Convert.ToInt32(dataIdAttribute.Value);
                                    }
                                }

                                //Loop through each li

                                HtmlNodeCollection columnsOfTableRow = tableRow.SelectNodes("td");

                                foreach (HtmlNode columnNode in columnsOfTableRow)
                                {
                                    if (columnNode.HasAttributes)
                                    {
                                        HtmlAttribute childNodeAttribute = columnNode.Attributes["class"];

                                        //Find download link
                                        if (childNodeAttribute.Value.SafeEquals("download-cell"))
                                        {
                                            HtmlNodeCollection anchorTagNodes = columnNode.SelectNodes("a");

                                            foreach (HtmlNode anchorTagNode in anchorTagNodes)
                                            {
                                                if (anchorTagNode.HasAttributes)
                                                {
                                                    HtmlAttribute downloadLinkAttribute = anchorTagNode.Attributes["href"];

                                                    if (downloadLinkAttribute != null)
                                                    {
                                                        string relativeDownloadPath = downloadLinkAttribute.Value;

                                                        relativeDownloadPath = relativeDownloadPath.Replace(
                                                            "subtitles", "subtitle");

                                                        relativeDownloadPath = string.Format(
                                                            SubtitleDownloadFormatLink, relativeDownloadPath);

                                                        subtitle.DownloadUrl = relativeDownloadPath;
                                                    }
                                                }
                                            }
                                        }

                                        //Find rating
                                        if (childNodeAttribute.Value.SafeEquals("rating-cell"))
                                        {
                                            HtmlNodeCollection spanNodes = columnNode.SelectNodes("span");

                                            foreach (HtmlNode spanNode in spanNodes)
                                            {
                                                subtitle.Rating = Convert.ToInt32(spanNode.InnerText);
                                            }
                                        }

                                        //Find language
                                        if (childNodeAttribute.Value.SafeEquals("flag-cell"))
                                        {
                                            HtmlNodeCollection spanNodes = columnNode.SelectNodes("span");

                                            foreach (HtmlNode spanNode in spanNodes)
                                            {
                                                if (spanNode.HasAttributes)
                                                {
                                                    HtmlAttribute subLangAttribute = spanNode.Attributes["class"];

                                                    if (subLangAttribute.Value.SafeEquals("sub-lang"))
                                                    {
                                                        subtitle.Language = spanNode.InnerText;
                                                    }
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
                            FileName = subtitle.FileName,
                            Language = subtitle.Language
                        };

                        dataFileCollection.DataFiles.Add(dataFile);
                    }
                }

                return dataFileCollection;
            }
        }
    }
}

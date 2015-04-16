using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using NDesk.Options;
using PlexHelper.YifySubtitles.Entities;
using PlexHelper.Yts.Entities;
using UtilsLib.ExtensionMethods;

namespace PlexHelper
{
    public class LogMessage
    {
        public DateTime TimeStamp { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", TimeStamp, Message);
        }
    }

    class Program
    {
        #region Properties

        public static bool Verbose
        {
            get
            {
                return Properties.Settings.Default.Verbose;
            }
        }

        public static bool CreateLogFile
        {
            get
            {
                return Properties.Settings.Default.CreateLogFile;
            }
        }

        public static List<LogMessage> LogFileContents = new List<LogMessage>();

        public static string[] SupportedMediaExtensions
        {
            get
            {
                return Properties.Settings.Default.SupportedMediaExtensions.Split("|");
            }
        }

        public static string[] SupportedSubtitleExtensions
        {
            get
            {
                return Properties.Settings.Default.SupportedSubtitleExtensions.Split("|");
            }
        }

        public static string StopCharacter
        {
            get
            {
                return Properties.Settings.Default.StopCharacter;
            }
        }

        public static List<PlexMediaOutputFile> MediaOutputFiles { get; set; }

        #endregion

        public static void LogMessage(string format, params object[] args)
        {
            if (Verbose)
            {
                Console.WriteLine(format, args);
            }

            if (CreateLogFile)
            {
                string message = string.Format(format, args);

                LogFileContents.Add(new LogMessage { Message = message, TimeStamp = DateTime.Now });
            }
        }


        public static void Main(string[] args)
        {
            string sourcePath = string.Empty;

            string destinationPath = string.Empty;

            bool downloadSubtitles = false;

            bool overwriteExistingSubtitles = false;

            bool showHelp = false;

            bool moveFiles = false;

            OptionSet p = new OptionSet
            {
                {"source=", "The directory containing the source files.", v => sourcePath = v},
                {"destination=", "The directory to save the output files.", v => destinationPath = v},
                {"dlsub", "Optional flag indicating to download subtitles for each source file.", v => downloadSubtitles = v != null},
                {"cleansub", "Optional flag indicating to clean existing subtitles for each source file.", v => overwriteExistingSubtitles = v != null},
                {"movefiles", "Optional flag indicating to move the files instead of copying them. Copy is on by default.", v => moveFiles = v != null},
                {"h|help", "Show this message and exit", v => showHelp = v != null },
            };

            List<string> extra;

            try
            {
                extra = p.Parse(args);
            }
            catch (Exception ex)
            {
                Console.Write("PlexHelper: ");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try `PlexHelper --help' for more information.");
                return;
            }

            if (showHelp || extra.Count > 0 || args.Length == 0)
            {
                ShowHelp(p);
                return;
            }

            if (Properties.Settings.Default.Debug)
            {
                Thread.Sleep(30000);
            }

            if (!Directory.Exists(sourcePath))
            {
                Environment.ExitCode = 1;

                Console.WriteLine("Source directory does not exist");

                return;
            }

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            LogMessage("Gathering media files");

            MediaOutputFiles = new List<PlexMediaOutputFile>();

            GatherMediaFiles(sourcePath);

            foreach (PlexMediaOutputFile mediaOutputFile in MediaOutputFiles)
            {
                LoadMediaFile(mediaOutputFile, destinationPath, downloadSubtitles);

                if (mediaOutputFile.IsValid)
                {
                    ProcessMediaFile(mediaOutputFile, moveFiles, overwriteExistingSubtitles);
                }
            }

            if (CreateLogFile)
            {
                string logfilePath = Path.Combine(sourcePath, string.Format("log_{0}.txt", DateTime.Now.ToString("MMddyyyyHHmmssffff")));

                if (LogFileContents.SafeAny())
                {
                    File.WriteAllLines(logfilePath, LogFileContents.Select(dd => dd.ToString()));
                }
            }

            if (Environment.ExitCode > 0)
            {
                LogMessage("Errors were encountered. Please check the logs for more information");
            }

            Console.WriteLine("FIN");
        }

        public static void CleanDirectoryAndSubfolders(string root, bool verbose = true, bool isTopmostRoot = false)
        {
            string[] files = Directory.GetFiles(root);

            foreach (string file in files)
            {
                try
                {
                    LogMessage("Deleting file: {0}", file);

                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    LogMessage("Stack Trace: {0}{1}{1}Message: {2}", ex.StackTrace, Environment.NewLine,
                            ex.Message);

                    Environment.ExitCode = 1;
                }
            }

            List<string> subDirectories = Directory.GetDirectories(root).ToList();

            foreach (string subDirectory in subDirectories)
            {
                CleanDirectoryAndSubfolders(subDirectory);
            }

            //At this point the directory should be clean of any files, delete it if it's not the root
            if (!isTopmostRoot)
            {
                try
                {
                    LogMessage("Deleting directory: {0}", root);

                    Directory.Delete(root);

                }
                catch (Exception ex)
                {
                    LogMessage("Stack Trace: {0}{1}{1}Message: {2}", ex.StackTrace, Environment.NewLine,
                            ex.Message);

                    Environment.ExitCode = 1;
                }
            }
        }

        static void ProcessMediaFile(PlexMediaOutputFile file, bool moveFiles, bool overwriteExistingSubtitles)
        {
            try
            {
                if (moveFiles)
                {
                    LogMessage("Moving file {0} to destination {1}", file.OriginalPath, file.NewPath);

                    File.Move(file.OriginalPath, file.NewPath);
                }
                else
                {
                    LogMessage("Copying file {0} to destination {1}", file.OriginalPath, file.NewPath);

                    File.Copy(file.OriginalPath, file.NewPath);
                }

                if (overwriteExistingSubtitles)
                {
                    //Delete existing subtitles
                    foreach (PlexMediaOutputMetaDataFile metaDataFile in file.MetaDataFiles.Where(dd => dd.Original))
                    {
                        LogMessage("Deleting original subtitle file: {0}", metaDataFile.OriginalPath);

                        File.Delete(metaDataFile.OriginalPath);
                    }
                }

                foreach (PlexMediaOutputMetaDataFile metaDataFile in file.MetaDataFiles.Where(dd => !dd.Original))
                {
                    LogMessage("Saving downloaded subtitle archive: {0}", metaDataFile.NewPath);

                    File.WriteAllBytes(metaDataFile.NewPath, metaDataFile.Data);

                    FileInfo zipFileInfo = new FileInfo(metaDataFile.NewPath);

                    string directory = zipFileInfo.DirectoryName;

                    if (string.IsNullOrWhiteSpace(directory))
                    {
                        directory = Directory.GetDirectoryRoot(metaDataFile.NewPath);
                    }

                    string temporaryPath = Path.Combine(directory, Guid.NewGuid().ToString());

                    LogMessage("Creating temporary directory to extract contents: {0}", temporaryPath);

                    Directory.CreateDirectory(temporaryPath);

                    LogMessage("Extracting contents of archive");

                    ZipFile.ExtractToDirectory(metaDataFile.NewPath, temporaryPath);

                    string[] filesExtracted = Directory.GetFiles(temporaryPath);

                    foreach (string s in filesExtracted)
                    {
                        FileInfo extractedFiles = new FileInfo(s);

                        if (SupportedSubtitleExtensions.Contains(extractedFiles.Extension))
                        {
                            string finalSubtitlePath = Path.Combine(directory,
                                string.Format("{0}{1}", metaDataFile.NewName, extractedFiles.Extension));

                            LogMessage("Saving final subtitle file: {0}", finalSubtitlePath);

                            File.Move(s, finalSubtitlePath);
                        }
                        else
                        {
                            File.Delete(s);
                        }
                    }

                    CleanDirectoryAndSubfolders(temporaryPath);

                    LogMessage("Deleting archive: {0}", metaDataFile.NewPath);

                    File.Delete(metaDataFile.NewPath);
                }
            }
            catch (Exception ex)
            {
                Environment.ExitCode = 1;

                LogMessage("Stack Trace: {0}{1}{1}Message: {2}", ex.StackTrace, Environment.NewLine,
                            ex.Message);
            }
        }

        static void LoadMediaFile(PlexMediaOutputFile file, string destination, bool downloadSubtitles)
        {
            try
            {
                LogMessage("Processing file: {0}", file.OriginalName);

                string searchName;

                if (file.OriginalDirectory.IndexOf(StopCharacter, StringComparison.Ordinal) > 0)
                {
                    searchName = file.OriginalDirectory.Substring(0, file.OriginalDirectory.IndexOf(StopCharacter, StringComparison.Ordinal));
                }
                else
                {
                    searchName = file.OriginalDirectory;
                }

                string[] pieces = searchName.Split(Path.DirectorySeparatorChar);

                searchName = pieces[pieces.Length - 1];

                LogMessage("Will use search name: {0}", searchName);

                YtsApiRequest request = new YtsApiRequest();

                LogMessage("Querying movie database...", searchName);

                YtsApiMovieResponse response = request.MovieListQueryAsync(searchName, exhaustiveSearch: true).Result;

                YtsMovie mostRelevantMovie =
                    response.MovieListResponse.Movies.OrderBy(dd => dd.FuzzyDistance).FirstOrDefault();

                if (mostRelevantMovie != null)
                {
                    file.IsValid = true;

                    LogMessage("Most relevant search result is: {0}", mostRelevantMovie.ToFormattedName());

                    file.NewName = mostRelevantMovie.ToFormattedName();

                    file.NewPath = Path.Combine(destination, string.Format("{0}{1}", file.NewName, file.Extension));

                    if (downloadSubtitles)
                    {
                        LogMessage("Downloading subtitles...");

                        SubtitleDownloader downloader = new SubtitleDownloader();

                        SubtitleDataFileCollection dataFileCollection =
                            downloader.DownloadSubtitlesAsync(mostRelevantMovie.ImdbCode).Result;

                        if (dataFileCollection != null)
                        {
                            foreach (SubtitleDataFile subtitleDataFile in dataFileCollection.DataFiles)
                            {
                                LogMessage("Subtitle file downloaded: {0}", subtitleDataFile.FileName);

                                PlexMediaOutputMetaDataFile metaDataFile = new PlexMediaOutputMetaDataFile
                                {
                                    Data = subtitleDataFile.Data,
                                    Extension = subtitleDataFile.Extension,
                                    NewName =
                                        string.Format("{0}{1}", mostRelevantMovie.ToFormattedName(),
                                            SubtitleLanguageCodeHelper.ConvertToLanguageCode(subtitleDataFile.Language)),
                                };

                                metaDataFile.NewPath = Path.Combine(destination,
                                    string.Format("{0}{1}", metaDataFile.NewName, metaDataFile.Extension));

                                file.MetaDataFiles.Add(metaDataFile);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No subtitles found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                file.IsValid = false;

                Environment.ExitCode = 1;

                LogMessage("Stack Trace: {0}{1}{1}Message: {2}", ex.StackTrace, Environment.NewLine,
                            ex.Message);
            }
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: PlexHelper [OPTIONS]+");
            Console.WriteLine("Renames media files to a specified format and downloads subtitles for each media file.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        static void GatherMediaFiles(string root, bool verbose = true, bool isTopmostRoot = false)
        {
            string[] files = Directory.GetFiles(root);

            PlexMediaOutputFile outputFile = null;

            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);

                if (SupportedMediaExtensions.Contains(info.Extension))
                {
                    LogMessage("Found supported file: {0}", info.FullName);

                    outputFile = new PlexMediaOutputFile
                    {
                        Extension = info.Extension,
                        OriginalDirectory = info.DirectoryName,
                        OriginalPath = info.FullName,
                        OriginalName = info.Name,
                    };
                }

                if (outputFile != null)
                {
                    if (SupportedSubtitleExtensions.Contains(info.Extension))
                    {
                        LogMessage("Found additional meta data files: {0}", info.FullName);

                        PlexMediaOutputMetaDataFile metaDataFile = new PlexMediaOutputMetaDataFile
                        {
                            Extension = info.Extension,
                            OriginalDirectory = info.DirectoryName,
                            OriginalPath = info.FullName,
                            Original = true,
                            OriginalName = info.Name
                        };

                        outputFile.MetaDataFiles.Add(metaDataFile);
                    }
                }
            }

            if (outputFile != null)
            {
                MediaOutputFiles.Add(outputFile);
            }

            List<string> subDirectories = Directory.GetDirectories(root).ToList();

            foreach (string subDirectory in subDirectories)
            {
                GatherMediaFiles(subDirectory);
            }
        }
    }
}

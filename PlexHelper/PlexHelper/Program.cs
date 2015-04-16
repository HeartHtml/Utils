using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk.Options;
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

        public static string[] SupportedExtensions
        {
            get
            {
                return Properties.Settings.Default.SupportedExtensions.Split("|");
            }
        }

        public static string[] StopCharacters
        {
            get
            {
                return Properties.Settings.Default.StopCharacters.Split(",");
            }
        }

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

            OptionSet p = new OptionSet
            {
                {"source=", "The directory containing the source files.", v => sourcePath = v},
                {"destination=", "The directory to save the output files.", v => destinationPath = v},
                {"dlsub", "Optional flag indicating to download subtitles for each source file.", v => downloadSubtitles = v != null},
                {"cleansub", "Optional flag indicating to clean existing subtitles for each source file.", v => overwriteExistingSubtitles = v != null},
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

            if (CreateLogFile)
            {
                string logfilePath = Path.Combine(sourcePath, string.Format("log_{0}.txt", DateTime.Now.ToString("MMddyyyyHHmmssffff")));

                if (LogFileContents.SafeAny())
                {
                    File.WriteAllLines(logfilePath, LogFileContents.Select(dd => dd.ToString()));
                }
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
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Mp3Lib;
using NDesk.Options;
using UtilsLib.ExtensionMethods;

namespace PlaylistGenerator
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

    public class PlaylistThread
    {
        public List<string> FilesToProcess { get; set; }

        public string PlaylistDirectory { get; set; }

        public WaitHandle DoneEvent { get; set; }

        public bool ForceRelativePaths { get; set; }

        public bool Artists { get; set; }

        public bool Albums { get; set; }

        public int ExitCode { get; set; }

        public bool Verbose { get; set; }

        public bool CreateLogFile { get; set; }

        public string AlbumSize { get; set; }

        public int MinimumAlbumSize { get; set; }

        public List<LogMessage> LogFileContents { get; set; }

        public PlaylistThread()
        {
            LogFileContents = new List<LogMessage>();

            FilesToProcess = new List<string>();
        }

        public void LogMessage(string format, params object[] args)
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

        public void ThreadPoolCallback(Object threadContext)
        {
            try
            {
                List<Mp3File> mp3Files = new List<Mp3File>();

                foreach (string song in FilesToProcess)
                {
                    try
                    {
                        Mp3File mp3File = new Mp3File(song);

                        mp3Files.Add(mp3File);
                    }
                    catch (Exception ex)
                    {
                        LogMessage("Could not load tag information for file: {0}", song);
                        LogMessage(ex.Message);
                        ExitCode = 1;
                    }
                }

                if (Artists)
                {
                    List<string> distinctArtists = new List<string>();

                    foreach (Mp3File mp3File in mp3Files)
                    {
                        try
                        {
                            if (!distinctArtists.Contains(mp3File.TagHandler.Artist))
                            {
                                distinctArtists.Add(mp3File.TagHandler.Artist);

                                LogMessage("Loaded artist tag information for file: {0}", mp3File.FileName);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogMessage("Could not load artist tag information for file: {0}", mp3File.FileName);
                            LogMessage(ex.Message);
                            ExitCode = 1;
                        }
                    }

                    string artistSubDirectory = Path.Combine(PlaylistDirectory, "Artists");

                    Directory.CreateDirectory(artistSubDirectory);

                    LogMessage("{0} distinct artists found", distinctArtists.Count);

                    foreach (string artist in distinctArtists)
                    {
                        LogMessage("Processing artist: {0}", artist);

                        List<Mp3File> songsForArtist = new List<Mp3File>();

                        foreach (Mp3File file in mp3Files)
                        {
                            try
                            {
                                if (file.TagHandler.Artist.SafeEquals(artist))
                                {
                                    songsForArtist.Add(file);
                                    LogMessage("Loading song {0} for artist {1}", file.FileName, artist);
                                }
                            }
                            catch (Exception ex)
                            {
                                ExitCode = 1;
                            }
                        }

                        List<string> fileNames =
                            songsForArtist.OrderBy(dd => dd.TagHandler.Song).Select(dd => dd.FileName).ToList();

                        List<string> filesOnThePlaylist = new List<string>();

                        if (ForceRelativePaths)
                        {
                            //TODO
                        }
                        else
                        {
                            filesOnThePlaylist.AddRange(fileNames);
                        }

                        string playlistFileName = Path.Combine(artistSubDirectory,
                            string.Format("{0}.m3u", artist.RemoveInvalidCharacters()));

                        LogMessage("Creating playlist: {0}", playlistFileName);

                        File.WriteAllLines(playlistFileName, filesOnThePlaylist);
                    }
                }

                if (Albums)
                {
                    List<string> distinctAlbums = new List<string>();

                    foreach (Mp3File mp3File in mp3Files)
                    {
                        try
                        {
                            if (!distinctAlbums.Contains(mp3File.TagHandler.Album))
                            {
                                distinctAlbums.Add(mp3File.TagHandler.Album);

                                LogMessage("Loaded album tag information for file: {0}", mp3File.FileName);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogMessage("Could not load album tag information for file: {0}", mp3File.FileName);
                            LogMessage(ex.Message);
                            ExitCode = 1;
                        }
                    }

                    string albumsSubDirectory = Path.Combine(PlaylistDirectory, "Albums");

                    Directory.CreateDirectory(albumsSubDirectory);

                    LogMessage("{0} distinct albums found", distinctAlbums.Count);

                    foreach (string album in distinctAlbums)
                    {
                        LogMessage("Processing album: {0}", album);

                        List<Mp3File> songsForAlbum = new List<Mp3File>();

                        foreach (Mp3File file in mp3Files)
                        {
                            try
                            {
                                if (file.TagHandler.Album.SafeEquals(album))
                                {
                                    songsForAlbum.Add(file);
                                    LogMessage("Loading song {0} for album {1}", file.FileName, album);
                                }
                            }
                            catch (Exception ex)
                            {
                                ExitCode = 1;
                            }
                        }

                        if (!AlbumSize.IsNullOrWhiteSpace())
                        {
                            if (songsForAlbum.Count < MinimumAlbumSize)
                            {
                                LogMessage("Skipping album {0}, number of songs less than minimum album size", album);
                                continue;
                            }
                        }

                        string artist = songsForAlbum.Select(dd => dd.TagHandler.Artist).FirstOrDefault();

                        List<string> fileNames =
                            songsForAlbum.OrderBy(dd => dd.TagHandler.Track).Select(dd => dd.FileName).ToList();

                        List<string> filesOnThePlaylist = new List<string>();

                        if (ForceRelativePaths)
                        {
                            //TODO
                        }
                        else
                        {
                            filesOnThePlaylist.AddRange(fileNames);
                        }

                        string playlistFileName = Path.Combine(albumsSubDirectory,
                            string.Format("{0} - {1}.m3u", artist.RemoveInvalidCharacters(),
                                album.RemoveInvalidCharacters()));

                        LogMessage("Creating playlist: {0}", playlistFileName);

                        File.WriteAllLines(playlistFileName, filesOnThePlaylist);
                    }
                }

            }
            catch (Exception ex)
            {
                LogMessage("Thread failed to process: {0}", ex.Message);
            }
            finally
            {
                LogMessage("Thread finished processing");

                ManualResetEvent resetEvent = DoneEvent as ManualResetEvent;

                if (resetEvent != null)
                {
                    resetEvent.Set();
                }
            }
        }
    }

    public class Program
    {
        public static string[] SupportedExtensions
        {
            get
            {
                string[] extensions = ConfigurationManager.AppSettings["SupportedExtensions"].Split(",");

                return extensions;
            }
        }

        public static bool Verbose
        {
            get
            {
                bool verbose = Convert.ToBoolean(ConfigurationManager.AppSettings["Verbose"]);

                return verbose;
            }
        }

        public static bool CreateLogFile
        {
            get
            {
                bool logFile = Convert.ToBoolean(ConfigurationManager.AppSettings["CreateLogFile"]);

                return logFile;
            }
        }

        public static int ExitCode
        {
            get;
            set;
        }

        public static bool ForceRelativePaths
        {
            get;
            set;
        }

        public static List<LogMessage> LogFileContents
        {
            get;
            set;
        }

        public static int NumberOfThreads
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfThreads"]);
            }
        }

        static void Main(string[] args)
        {
            string rootDirectory = string.Empty;

            bool albums = false;

            bool artists = false;

            bool showHelp = false;

            bool cleanDirectoryFirst = false;

            string albumSize = string.Empty;

            OptionSet p = new OptionSet
            {
                {"root=", "The root directory to start the playlist creation.", v => rootDirectory = v},
                {"albums", "Flag indicating to create playlists for each album.", v => albums = v != null},
                {"artists", "Flag indicating to create playlists for each artist.", v => artists = v != null},
                {"albumsize=", "Optional number value indicating to only create album playlists for albums containing at least the specified number of tracks.", v => albumSize = v},
                //{"forcerelative", "Optional flag indicating to force relative paths for each file in the playlist.", v => ForceRelativePaths = v != null}, --Not ready yet
                {"clean", "Optional flag indicating to clean the playlist directory first.", v => cleanDirectoryFirst = v != null},
                {"h|help", "Show this message and exit", v => showHelp = v != null },
            };

            List<string> extra;

            try
            {
                extra = p.Parse(args);
            }
            catch (Exception ex)
            {
                Console.Write("PlaylistGenerator: ");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try `PlaylistGenerator --help' for more information.");
                return;
            }

            if (showHelp || extra.Count > 0 || args.Length == 0)
            {
                ShowHelp(p);
                return;
            }

            if (!artists && !albums)
            {
                ShowHelp(p);
                return;
            }

            int minimumAlbumSize = 0;

            if (!albumSize.IsNullOrWhiteSpace() && !int.TryParse(albumSize, out minimumAlbumSize))
            {
                ShowHelp(p);
                return;
            }

            ExitCode = 0;

            LogFileContents = new List<LogMessage>();

            string playlistDirectory = Path.GetPathRoot(rootDirectory);

            if (string.IsNullOrWhiteSpace(playlistDirectory))
            {
                playlistDirectory = rootDirectory;
            }
            else
            {
                playlistDirectory = Path.Combine(playlistDirectory, "Playlists");
            }

            string logfilePath = Path.Combine(playlistDirectory, string.Format("log_{0}.txt", DateTime.Now.ToString("MMddyyyyHHmmssffff")));

            try
            {
                if (!Directory.Exists(rootDirectory))
                {
                    Console.WriteLine("Root directory does not exist");

                    if (CreateLogFile)
                    {
                        File.WriteAllText(logfilePath, string.Format("Root directory does not exist: {0}", rootDirectory));
                    }

                    ExitCode = 1;

                    return;
                }

                if (Directory.Exists(playlistDirectory))
                {
                    if (cleanDirectoryFirst)
                    {
                        CleanDirectoryAndSubfolders(playlistDirectory, verbose: Verbose, isTopmostRoot: true);
                    }
                }

                if (!Directory.Exists(playlistDirectory))
                {
                    Directory.CreateDirectory(playlistDirectory);
                }

                List<string> songs = new List<string>();

                LoadAllFiles(rootDirectory, songs);

                if (songs.Count > 0)
                {
                    WaitHandle[] doneEvents = new WaitHandle[NumberOfThreads];

                    List<IEnumerable<string>> chunks = songs.SplitIntoChunks(NumberOfThreads).ToList();

                    for (int i = 0; i < NumberOfThreads; i ++)
                    {
                        doneEvents[i] = new ManualResetEvent(false);

                        PlaylistThread thread = new PlaylistThread
                        {
                            AlbumSize = albumSize,
                            Albums = albums,
                            Artists = artists,
                            CreateLogFile = CreateLogFile,
                            DoneEvent = doneEvents[i],
                            ExitCode = ExitCode,
                            ForceRelativePaths = ForceRelativePaths,
                            LogFileContents = LogFileContents,
                            MinimumAlbumSize = minimumAlbumSize,
                            PlaylistDirectory = playlistDirectory,
                            Verbose = Verbose,
                            FilesToProcess = chunks[i].ToList()
                        };

                        ThreadPool.QueueUserWorkItem(thread.ThreadPoolCallback, i);
                    }

                    WaitHandle.WaitAll(doneEvents);

                    LogMessage("All threads done processing");
                }
            }
            catch (Exception ex)
            {
                LogMessage("Stack Trace: {0}{1}{1}Message: {2}", ex.StackTrace, Environment.NewLine, ex.Message);

                ExitCode = 1;
            }

            if (CreateLogFile)
            {
                List<string> logContents = new List<string>();

                logContents.AddRange(LogFileContents.OrderBy(dd => dd.TimeStamp).Select(dd => dd.ToString()));

                File.WriteAllLines(logfilePath, logContents);
            }

            if (ExitCode > 0)
            {
                Console.WriteLine("Some errors were encountered. Please review the logs for more information");
            }

            Console.WriteLine("FIN");

            Environment.ExitCode = ExitCode;
        }

        public static void LoadAllFiles(string root, List<string> songs, bool verbose = true)
        {
            string[] files = Directory.GetFiles(root);

            foreach (string file in files)
            {
                try
                {
                    FileInfo info = new FileInfo(file);

                    if (SupportedExtensions.Contains(info.Extension))
                    {
                        LogMessage("Adding file: {0}", file);

                        string fileToAdd = file;

                        songs.Add(fileToAdd);
                    }
                    else
                    {
                        LogMessage("Skipping file, unsupported extension: {0}", file);
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("Stack Trace: {0}{1}{1}Message: {2}", ex.StackTrace, Environment.NewLine,
                            ex.Message);

                    ExitCode = 1;
                }
            }

            List<string> subDirectories = Directory.GetDirectories(root).ToList();

            foreach (string subDirectory in subDirectories)
            {
                LoadAllFiles(subDirectory, songs);
            }
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

                    ExitCode = 1;
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

                    ExitCode = 1;
                }
            }
        }

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

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: PlaylistGenerator [OPTIONS]+");
            Console.WriteLine("Create playlists from a source directory.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}

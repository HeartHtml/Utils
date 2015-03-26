using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Mp3Lib;
using NDesk.Options;
using UtilsLib.ExtensionMethods;

namespace PlaylistGenerator
{
    class Program
    {
        public static string[] SupportedExtensions
        {
            get
            {
                string[] extensions = ConfigurationManager.AppSettings["SupportedExtensions"].Split(",");

                return extensions;
            }
        }

        public static int ExitCode
        {
            get; set;
        }

        public static bool ForceRelativePaths
        {
            get; set;
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

            try
            {
                if (!File.Exists(rootDirectory))
                {
                    Console.WriteLine("Root directory does not exist");

                    ExitCode = 1;
                }

                string playlistDirectory = Path.GetPathRoot(rootDirectory);

                if (string.IsNullOrWhiteSpace(playlistDirectory))
                {
                    playlistDirectory = rootDirectory;
                }
                else
                {
                    playlistDirectory = Path.Combine(playlistDirectory, "Playlists");
                }

                if (Directory.Exists(playlistDirectory))
                {
                    if (cleanDirectoryFirst)
                    {
                        CleanDirectoryAndSubfolders(playlistDirectory, verbose: true, isTopmostRoot: true);
                    }
                }

                if (!Directory.Exists(playlistDirectory))
                {
                    Directory.CreateDirectory(playlistDirectory);
                }

                List<string> songs = new List<string>();

                LoadAllFiles(rootDirectory, songs);

                List<Mp3File> mp3Files = songs.Select(dd => new Mp3File(dd)).ToList();

                if (artists)
                {
                    List<string> distinctArtists = mp3Files.Select(dd => dd.TagHandler.Artist).Distinct().ToList();

                    string artistSubDirectory = Path.Combine(playlistDirectory, "Artists");

                    Directory.CreateDirectory(artistSubDirectory);

                    Console.WriteLine("{0} distinct artists found", distinctArtists.Count);

                    foreach (string artist in distinctArtists)
                    {
                        Console.WriteLine("Processing artist: {0}", artist);

                        List<Mp3File> songsForArtist =
                            mp3Files.Where(dd => dd.TagHandler.Artist.SafeEquals(artist)).ToList();

                        List<string> fileNames = songsForArtist.OrderBy(dd => dd.TagHandler.Song).Select(dd => dd.FileName).ToList();

                        List<string> filesOnThePlaylist = new List<string>();

                        if (ForceRelativePaths)
                        {
                            //TODO
                        }
                        else
                        {
                            filesOnThePlaylist.AddRange(fileNames);
                        }

                        string playlistFileName = Path.Combine(artistSubDirectory, string.Format("{0}.m3u", artist.RemoveInvalidCharacters()));

                        Console.WriteLine("Creating playlist: {0}", playlistFileName);

                        File.WriteAllLines(playlistFileName, filesOnThePlaylist);
                    }
                }

                if (albums)
                {
                    List<string> distinctAlbums = mp3Files.Select(dd => dd.TagHandler.Album).Distinct().ToList();

                    string albumsSubDirectory = Path.Combine(playlistDirectory, "Albums");

                    Directory.CreateDirectory(albumsSubDirectory);

                    Console.WriteLine("{0} distinct albums found", distinctAlbums.Count);

                    foreach (string album in distinctAlbums)
                    {
                        Console.WriteLine("Processing album: {0}", album);

                        List<Mp3File> songsForAlbum =
                            mp3Files.Where(dd => dd.TagHandler.Album.SafeEquals(album)).ToList();

                        if (!albumSize.IsNullOrWhiteSpace())
                        {
                            if (songsForAlbum.Count < minimumAlbumSize)
                            {
                                Console.WriteLine("Skipping album {0}, number of songs less than minimum album size", album);
                                continue;
                            }
                        }

                        string artist = songsForAlbum.Select(dd => dd.TagHandler.Artist).FirstOrDefault();

                        List<string> fileNames = songsForAlbum.OrderBy(dd => dd.TagHandler.Track).Select(dd => dd.FileName).ToList();

                        List<string> filesOnThePlaylist = new List<string>();

                        if (ForceRelativePaths)
                        {
                            //TODO
                        }
                        else
                        {
                            filesOnThePlaylist.AddRange(fileNames);
                        }

                        string playlistFileName = Path.Combine(albumsSubDirectory, string.Format("{0} - {1}.m3u", artist.RemoveInvalidCharacters(), album.RemoveInvalidCharacters()));

                        Console.WriteLine("Creating playlist: {0}", playlistFileName);

                        File.WriteAllLines(playlistFileName, filesOnThePlaylist);
                    }
                }

                Console.WriteLine("FIN");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Stack Trace: {0}{1}{1}Message: {2}",ex.StackTrace, Environment.NewLine, ex.Message);

                ExitCode = 1;
            }

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
                        if (verbose)
                        {
                            Console.WriteLine("Adding file: {0}", file);
                        }

                        string fileToAdd = file;

                        songs.Add(fileToAdd);
                    }
                    else
                    {
                        if (verbose)
                        {
                            Console.WriteLine("Skipping file, unsupported extension: {0}", file);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Stack Trace: {0}{1}{1}Message: {2}", ex.StackTrace, Environment.NewLine, ex.Message);

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
                    if (verbose)
                    {
                        Console.WriteLine("Deleting file: {0}", file);
                    }

                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Stack Trace: {0}{1}{1}Message: {2}", ex.StackTrace, Environment.NewLine, ex.Message);

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
                    if (verbose)
                    {
                        Console.WriteLine("Deleting directory: {0}", root);
                    }

                    Directory.Delete(root);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Stack Trace: {0}{1}{1}Message: {2}", ex.StackTrace, Environment.NewLine, ex.Message);

                    ExitCode = 1;
                }
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

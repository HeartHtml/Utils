using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlexPlaylistExporter
{


    class Program
    {
        static void Main(string[] args)
        {
            IAppSettings appSettings = new AppSettings(ConfigurationManager.AppSettings);

            if (appSettings.Debug)
            {
                Thread.Sleep(20000);
            }

            SQLiteConnection plexDbConnection = new SQLiteConnection($"Data Source={appSettings.PlexDbLocation}");

            plexDbConnection.Open();

            SQLiteCommand command = new SQLiteCommand(appSettings.ExportQuery, plexDbConnection);

            List<PlexQueryPlaylistResult> results = new List<PlexQueryPlaylistResult>();

            Console.WriteLine("Querying plex database...");
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                results.Add(new PlexQueryPlaylistResult { FileName = reader["file"].ToString(), Title = reader["title"].ToString() });
            }

            List<Playlist> playListsToSave =
                Playlist.CreatePlaylists(results, appSettings.PlaylistExtension, appSettings.SaveLocation).ToList();

            if (playListsToSave.Count > 0)
            {
                Console.WriteLine("Saving playlists...");

                if (!Directory.Exists(appSettings.SaveLocation))
                {
                    Directory.CreateDirectory(appSettings.SaveLocation);
                }

                foreach (Playlist playlist in playListsToSave)
                {
                    File.WriteAllLines(playlist.FileName, playlist.Items.Select(dd => dd.FileName).ToArray());
                }
            }
            else
            {
                Console.WriteLine("No playlists to save");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexPlaylistExporter
{

    public class Playlist
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public string Extension { get; set; }

        public List<PlaylistItem> Items { get; set; }

        public Playlist()
        {
            Items = new List<PlaylistItem>();
        }

        public static IEnumerable<Playlist> CreatePlaylists(IEnumerable<PlexQueryPlaylistResult> results, string extension, string saveLocation)
        {
            List<string> uniquePlaylists = results.Select(dd => dd.Title).Distinct().ToList();

            List<Playlist> playlists = new List<Playlist>();

            foreach (string uniquePlaylist in uniquePlaylists)
            {
                List<string> songsForPlaylist =
                    results.Where(dd => dd.Title.Equals(uniquePlaylist)).Select(dd => dd.FileName).ToList();

                playlists.Add(new Playlist
                {
                    Title = uniquePlaylist,
                    Items = songsForPlaylist.Select(dd => new PlaylistItem { FileName = dd }).ToList(),
                    Extension = extension,
                    FileName = $"{saveLocation}\\{uniquePlaylist}.{extension}"
                });
            }

            return playlists;
        }
    }
}

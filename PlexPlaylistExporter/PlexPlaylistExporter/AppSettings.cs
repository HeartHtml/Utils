using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexPlaylistExporter
{
    public class AppSettings : IAppSettings
    {
        public NameValueCollection ConfigSettings { get; }

        public AppSettings(NameValueCollection configSettings)
        {
            ConfigSettings = configSettings;
        }

        public string PlexDbLocation => ConfigSettings["PlexDbLocation"];

        public string ExportQuery => ConfigSettings["ExportQuery"];

        public string PlaylistExtension => ConfigSettings["PlaylistExtension"];

        public string SaveLocation => ConfigSettings["SaveLocation"];

        public bool Debug => Convert.ToBoolean(ConfigSettings["Debug"]);
    }
}

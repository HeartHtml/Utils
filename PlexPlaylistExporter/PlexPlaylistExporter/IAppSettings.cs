using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexPlaylistExporter
{
    public interface IAppSettings
    {
        string PlexDbLocation { get; }

        string ExportQuery { get;  }

        string PlaylistExtension { get; }

        string SaveLocation { get; }

        bool Debug { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexHelper.YifySubtitles.Entities
{
    public class Subtitle
    {
        public int Rating { get; set; }

        public string Language { get; set; }

        public int DataId { get; set; }

        public string DownloadUrl { get; set; }

        public string FileName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexHelper
{
    public class PlexMediaOutputMetaDataFile
    {
        public string OriginalPath { get; set; }

        public string NewPath { get; set; }

        public string NewName { get; set; }

        public string OriginalDirectory { get; set; }

        public string Extension { get; set; }

        public string OriginalName { get; set; }

        public bool IsOriginal { get; set; }

        public byte[] Data { get; set; }
    }
}

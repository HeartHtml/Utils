using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexHelper
{
    public class PlexMediaOutputFile
    {
        public string OriginalPath { get; set; }

        public string NewPath { get; set; }

        public string NewName { get; set; }

        public string OriginalDirectory { get; set; }

        public string Extension { get; set; }

        public string OriginalName { get; set; }

        public bool IsValid { get; set; }

        public List<PlexMediaOutputMetaDataFile> MetaDataFiles { get; set; }

        public PlexMediaOutputFile()
        {
            MetaDataFiles = new List<PlexMediaOutputMetaDataFile>();
        }
    }
}

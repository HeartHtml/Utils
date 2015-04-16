using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexHelper.YifySubtitles.Entities
{
    public class SubtitleDataFileCollection
    {
        public List<SubtitleDataFile> DataFiles { get; set; }

        public SubtitleDataFileCollection()
        {
            DataFiles = new List<SubtitleDataFile>();
        }

        public override string ToString()
        {
            return string.Format("Data File Count: {0}", DataFiles.Count);
        }
    }
}

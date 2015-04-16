using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexHelper.YifySubtitles.Entities
{
    public class SubtitleDataFile
    {
        public byte[] Data { get; set; }

        public string Extension { get; set; }

        public string FileName { get; set; }

        public string Language { get; set; }

        public override string ToString()
        {
            return string.Format("File Name: {0}, Extension: {1}", FileName, Extension);
        }
    }
}

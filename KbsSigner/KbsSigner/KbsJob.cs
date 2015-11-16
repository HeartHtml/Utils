using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsLib.ExtensionMethods;

namespace KbsSigner
{
    public class KbsJob
    {
        public string KbsFilePath { get; set; }

        public string DestinationFilePath { get; set; }

        public KbsJobMetaData MetaData { get; set; }

        public List<string> JobContents { get; set; } 

        public KbsJob(string filePath, string metaDataFilePath)
        {
            KbsFilePath = filePath;

            FileInfo info = new FileInfo(filePath);

            string directory = Environment.SystemDirectory;

            if (info.DirectoryName != null)
            {
                directory = info.DirectoryName;
            }

            string newPath = Path.Combine(directory, string.Format("{0}.pdf", info.Name.Split(".")[0]));

            DestinationFilePath = newPath;

            ParseMetaData(metaDataFilePath);

            ParseLines();
        }

        public KbsJob(string filePath, string metaDataFilePath, string destinationFilePath)
        {
            KbsFilePath = filePath;

            DestinationFilePath = destinationFilePath;

            ParseMetaData(metaDataFilePath);

            ParseLines();
        }

        private void ParseMetaData(string metaFilePath)
        {
            try
            {
                MetaData = new KbsJobMetaData(metaFilePath);
            }
            catch (Exception ex)
            {
                MetaData = new KbsJobMetaData {ParseErrorMessage = ex.Message};
            }
        }

        private void ParseLines()
        {
            JobContents = File.ReadAllLines(KbsFilePath).ToList();
        }
    }
}

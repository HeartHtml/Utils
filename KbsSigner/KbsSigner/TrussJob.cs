using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsLib.ExtensionMethods;

namespace TrussSigner
{
    public class TrussJob
    {
        public string TrussJobFilePath { get; set; }

        public string DestinationFilePath { get; set; }

        public TrussJobMetaData MetaData { get; set; }

        public List<string> JobContents { get; set; } 

        public bool RemoveHeaderLine { get; set; }

        public TrussJob(string filePath, string metaDataFilePath, bool removeHeaderLine = true)
        {
            TrussJobFilePath = filePath;

            FileInfo info = new FileInfo(filePath);

            string directory = Environment.SystemDirectory;

            if (info.DirectoryName != null)
            {
                directory = info.DirectoryName;
            }

            string newPath = Path.Combine(directory, string.Format("{0}.pdf", info.Name.Split(".")[0]));

            DestinationFilePath = newPath;

            RemoveHeaderLine = removeHeaderLine;

            ParseMetaData(metaDataFilePath);

            ParseLines();
        }

        public TrussJob(string filePath, string metaDataFilePath, string destinationFilePath, bool removeHeaderLine = true)
        {
            TrussJobFilePath = filePath;

            DestinationFilePath = destinationFilePath;

            RemoveHeaderLine = removeHeaderLine;

            ParseMetaData(metaDataFilePath);

            ParseLines();
        }

        private void ParseMetaData(string metaFilePath)
        {
            try
            {
                MetaData = new TrussJobMetaData(metaFilePath);
            }
            catch (Exception ex)
            {
                MetaData = new TrussJobMetaData {ParseErrorMessage = ex.Message};
            }
        }

        private void ParseLines()
        {
            JobContents = File.ReadAllLines(TrussJobFilePath).ToList();

            //Special case to deal with .pcl truss files
            if (RemoveHeaderLine && TrussJobFilePath.Contains(".pcl"))
            {
                JobContents.RemoveAt(0);
            }
        }
    }
}

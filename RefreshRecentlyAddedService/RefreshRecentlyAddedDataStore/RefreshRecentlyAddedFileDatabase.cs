using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RefreshRecentlyAddedLogger;

namespace RefreshRecentlyAddedDataStore
{
    public class RefreshRecentlyAddedFileDatabase
    {
        private readonly Logger _logger = new Logger();

        private List<RefreshRecentlyAddedFileEntry> _files;

        public List<RefreshRecentlyAddedFileEntry> Files
        {
            get
            {
                return _files;
            }
            set
            {
                _files = value;
            }
        }

        public string DatabasePath { get; private set; }

        public RefreshRecentlyAddedFileDatabase(string fullPath)
        {
            InitializeDatabase(fullPath);

            DatabasePath = fullPath;
        }

        protected void InitializeDatabase(string fullPath)
        {
            Files = new List<RefreshRecentlyAddedFileEntry>();

            if (File.Exists(fullPath))
            {
                Files = ParseDbFile(fullPath).ToList();
            }
        }

        public void SaveDatabase()
        {
            try
            {
                if (File.Exists(DatabasePath))
                {
                    File.Delete(DatabasePath);
                }

                List<string> fileLines = new List<string>();

                foreach (RefreshRecentlyAddedFileEntry file in Files.OrderByDescending(dd => dd.PlayCount))
                {
                    string fileLine = string.Format("{0}|{1}", file.FileName, file.PlayCount);

                    fileLines.Add(fileLine);
                }

                File.WriteAllLines(DatabasePath, fileLines);
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }

        protected IEnumerable<RefreshRecentlyAddedFileEntry> ParseDbFile(string fullPath)
        {
            List<RefreshRecentlyAddedFileEntry> files = new List<RefreshRecentlyAddedFileEntry>();

            try
            {
                string[] lines = File.ReadAllLines(fullPath);

                foreach (string line in lines)
                {
                    RefreshRecentlyAddedFileEntry fileEntry = new RefreshRecentlyAddedFileEntry
                    {
                        FileName = line.Split('|')[0],
                        PlayCount = Convert.ToInt32(line.Split('|')[1])
                    };

                    files.Add(fileEntry);
                }
            }
            catch (Exception e)
            {
                files = new List<RefreshRecentlyAddedFileEntry>();
                _logger.LogException(e);
            }

            return files;
        }
    }
}

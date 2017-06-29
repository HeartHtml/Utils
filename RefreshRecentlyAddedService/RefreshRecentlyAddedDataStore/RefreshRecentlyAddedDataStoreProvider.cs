using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefreshRecentlyAddedDataStore
{
    public class RefreshRecentlyAddedDataStoreProvider
    {
        public RefreshRecentlyAddedFileDatabase Database { get; private set; }

        protected int RandomFilePlayCountThresholdUntilDelete { get; set; }

        protected int MinimumPlayCountAllowed { get; set; }

        protected RefreshRecentlyAddedFileEntry GetEntry(string fullPath)
        {
            RefreshRecentlyAddedFileEntry entry = Database.Files.FirstOrDefault(dd => dd.FileName.Equals(fullPath));

            return entry;
        }

        protected void CleanupDatabase()
        {
            foreach (RefreshRecentlyAddedFileEntry entry in Database.Files)
            {
                if (entry.PlayCount > RandomFilePlayCountThresholdUntilDelete)
                {
                    entry.FileName = "DELETE";
                }
            }

            Database.Files.RemoveAll(dd => dd.FileName.Equals("DELETE"));
        }

        public RefreshRecentlyAddedDataStoreProvider(string fullPath, int randomFilePlayCountThresholdUntilDelete, int minimumPlayCountAllowed)
        {
            Database = new RefreshRecentlyAddedFileDatabase(fullPath);

            RandomFilePlayCountThresholdUntilDelete = randomFilePlayCountThresholdUntilDelete;

            MinimumPlayCountAllowed = minimumPlayCountAllowed;
        }

        public void PersistDatabase()
        {
            CleanupDatabase();

            Database.SaveDatabase();
        }

        public bool IsFileAllowed(string fullPath)
        {
            RefreshRecentlyAddedFileEntry entry = GetEntry(fullPath);

            bool allowed = true;

            if (entry != null)
            {
                allowed = entry.PlayCount < MinimumPlayCountAllowed;
            }

            return allowed;
        }

        public void IncreasePlayCount(string fullPath)
        {
            RefreshRecentlyAddedFileEntry entry = GetEntry(fullPath);

            if (entry == null)
            {
                entry = new RefreshRecentlyAddedFileEntry
                {
                    PlayCount = 1,
                    FileName = fullPath
                };

                Database.Files.Add(entry);
            }
            else
            {
                entry.PlayCount++;
            }

            CleanupDatabase();
        }
    }
}

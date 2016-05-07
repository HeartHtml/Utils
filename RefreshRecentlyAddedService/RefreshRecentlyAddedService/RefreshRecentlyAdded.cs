using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace RefreshRecentlyAddedService
{
    public class RefreshRecentlyAdded
    {

        public bool Debug
        {
            get
            {
                return Properties.Settings.Default.Debug;
            }
        }

        public int DebugWaitTime
        {
            get
            {
                return Properties.Settings.Default.DebugWaitTime;
            }
        }

        public bool RunRecentlyAddedRoutine
        {
            get
            {
                return Properties.Settings.Default.RunRecentlyAddedRoutine;
            }
        }

        public bool RunRandomPlaylistRoutine
        {
            get
            {
                return Properties.Settings.Default.RunRandomPlaylistRoutine;
            }
        }

        public int RandomPlaylistRefreshTimeIntervalInDays { get; set; }

        public int RefreshTimeIntervalInDays { get; set; }

        public int RefreshRateInSeconds { get; set; }

        public int NumberOfRandomFiles { get; set; }

        public string RecentlyAddedLocation { get; set; }

        public string ScanLocation { get; set; }

        public string RandomPlaylistLocation { get; set; }

        private Thread RefreshThread;

        public RefreshRecentlyAdded()
        {
            
        }

        public RefreshRecentlyAdded(int RefreshTimeIntervalInDays,
                                    int RefreshRateInSeconds,
                                    int RandomPlaylistRefreshTimeIntervalInDays,
                                    int NumberOfRandomFiles,
                                    string RecentlyAddedLocation,
                                    string ScanLocation,
                                    string RandomPlaylistLocation)
        {
            this.RandomPlaylistRefreshTimeIntervalInDays = RandomPlaylistRefreshTimeIntervalInDays;
            this.RefreshTimeIntervalInDays = RefreshTimeIntervalInDays;
            this.RefreshRateInSeconds = RefreshRateInSeconds;
            this.RecentlyAddedLocation = RecentlyAddedLocation;
            this.ScanLocation = ScanLocation;
            this.RandomPlaylistLocation = RandomPlaylistLocation;
            this.NumberOfRandomFiles = NumberOfRandomFiles;
            RefreshThread = new Thread(RefreshFiles);
        }

        public void StopRefreshing()
        {
            if (RefreshThread.ThreadState != System.Threading.ThreadState.AbortRequested &&
                RefreshThread.ThreadState != System.Threading.ThreadState.StopRequested &&
                RefreshThread.ThreadState != System.Threading.ThreadState.SuspendRequested &&
                RefreshThread.ThreadState != System.Threading.ThreadState.Stopped &&
                RefreshThread.ThreadState != System.Threading.ThreadState.Unstarted)
            {
                RefreshThread.Abort();
            }
        }

        public void StartRefreshing()
        {
            RefreshThread.Start();
        }

        public void RefreshFiles()
        {
            while (true)
            {
                if (Debug)
                {
                    Thread.Sleep(DebugWaitTime);
                }

                if (RunRandomPlaylistRoutine)
                {
                    CreateRandomFilesInLocationFunction(ScanLocation,
                        RandomPlaylistLocation,
                        RandomPlaylistRefreshTimeIntervalInDays);
                }

                if (RunRecentlyAddedRoutine)
                {
                    RefreshLocationFunction(ScanLocation,
                        RecentlyAddedLocation,
                        RefreshTimeIntervalInDays);
                }

                Thread.Sleep(RefreshRateInSeconds);
            }
        }

        private bool FileIsVideoFile(string Extension)
        {
            bool Result = false;
            if (GetValidMovieFileExtensions().Contains(Extension))
            {
                Result = true;
            }
            return Result;
        }

        private void LogExceptionInEventViewer(Exception e)
        {
            if (!EventLog.SourceExists("RefreshRecentlyAdded"))
            {
                EventLog.CreateEventSource("RefreshRecentlyAdded", "FatalError");
            }
            EventLog.WriteEntry("RefreshRecentlyAdded", e.Message, EventLogEntryType.Error);
        }

        private List<string> GetFileNames(string[] Files)
        {
            List<string> FileNames = new List<string>();
            foreach (string file in Files)
            {
                FileInfo Info = new FileInfo(file);
                FileNames.Add(Info.Name);
            }
            return FileNames;
        }

        private List<string> GetValidMovieFileExtensions()
        {
            List<string> validMovieFileExtensions = new List<string>();
            string extensionsFromConfig = Properties.Settings.Default.ValidMovieExtensions;
            string[] extensionsSplit = extensionsFromConfig.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in extensionsSplit)
            {
                validMovieFileExtensions.Add(s);
            }
            return validMovieFileExtensions;
        }

        public void CleanUpLocationFunction(string LocationToCleanUp,
                                            int RefreshTimeInterval)
        {
            try
            {
                if (!Directory.Exists(LocationToCleanUp))
                {
                    Directory.CreateDirectory(LocationToCleanUp);
                }

                string[] FilesInRecentlyAdded = Directory.GetFiles(LocationToCleanUp, "*.*", SearchOption.AllDirectories);

                foreach (string CurrentFile in FilesInRecentlyAdded)
                {
                    FileInfo InfoAboutFile = new FileInfo(CurrentFile);
                    if (!InfoAboutFile.Name.ToLower().Contains(".DS_Store".ToLower()))
                    {
                        if (DateTime.Today > InfoAboutFile.CreationTime.AddDays(RefreshTimeInterval).Date)
                        {
                            File.Delete(InfoAboutFile.FullName);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogExceptionInEventViewer(e);
            }
        }

        public void RefreshLocationFunction(string LocationOfSourceFiles,
                                                 string LocationToCopyTo,
                                                 int RefreshTimeInterval)
        {
            try
            {

                CleanUpLocationFunction(LocationToCopyTo,
                                        RefreshTimeIntervalInDays);

                if (!Directory.Exists(LocationToCopyTo))
                {
                    Directory.CreateDirectory(LocationToCopyTo);
                }

                string[] FilesInScanLocation = Directory.GetFiles(LocationOfSourceFiles, "*.*", SearchOption.AllDirectories);
                string[] FilesInDestination = Directory.GetFiles(LocationToCopyTo, "*.*", SearchOption.AllDirectories);

                List<String> FilesInScanLocationList = FilesInScanLocation.ToList();
                List<String> FilesInDestinationList = GetFileNames(FilesInDestination);

                FilesInScanLocationList.RemoveAll(ss => ss.Contains(RandomPlaylistLocation));
                FilesInScanLocationList.RemoveAll(ss => ss.Contains(RecentlyAddedLocation));

                foreach (string CurrentFile in FilesInScanLocationList)
                {
                    FileInfo InfoAboutFile = new FileInfo(CurrentFile);
                    if (!InfoAboutFile.Name.ToLower().Contains(".DS_Store".ToLower()) &&
                        FileIsVideoFile(InfoAboutFile.Extension))
                    {
                        if (FilesInDestinationList.Contains(InfoAboutFile.Name))
                        {
                            continue;
                        }
                        if (InfoAboutFile.CreationTime >= DateTime.Today.AddDays(RefreshTimeInterval * -1) &&
                            InfoAboutFile.CreationTime <= DateTime.Today.AddDays(1).AddMilliseconds(-1))
                        {
                            File.Copy(InfoAboutFile.FullName, Path.Combine(LocationToCopyTo, InfoAboutFile.Name));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogExceptionInEventViewer(e);
            }
        }

        public void CreateRandomFilesInLocationFunction(string LocationOfSourceFiles,
                                                        string LocationToCopyTo,
                                                        int RefreshTimeInterval)
        {
            try
            {
                CleanUpLocationFunction(LocationToCopyTo,
                                        RefreshTimeInterval);

                if (!Directory.Exists(LocationToCopyTo))
                {
                    Directory.CreateDirectory(LocationToCopyTo);
                }

                List<String> FilesInScanLocationList = Directory.GetFiles(LocationOfSourceFiles, "*.*", SearchOption.AllDirectories).ToList();

                FilesInScanLocationList.RemoveAll(ss => ss.Contains(RandomPlaylistLocation));
                FilesInScanLocationList.RemoveAll(ss => ss.Contains(RecentlyAddedLocation));

                Random r = new Random();
                while(Directory.GetFiles(LocationToCopyTo, "*.*", SearchOption.AllDirectories).Length < NumberOfRandomFiles)
                {
                    string RandomFileName = FilesInScanLocationList[r.Next(FilesInScanLocationList.Count)];
                    FileInfo InfoAboutFile = new FileInfo(RandomFileName);
                    if (GetFileNames(Directory.GetFiles(LocationToCopyTo, "*.*", SearchOption.AllDirectories)).Contains(InfoAboutFile.Name))
                    {
                        continue;
                    }
                    if (!InfoAboutFile.Name.ToLower().Contains(".DS_Store".ToLower()) &&
                        FileIsVideoFile(InfoAboutFile.Extension))
                    {
                        File.Copy(InfoAboutFile.FullName, Path.Combine(LocationToCopyTo, InfoAboutFile.Name));
                    }
                }
            }
            catch (Exception e)
            {
                LogExceptionInEventViewer(e);
            }

        }

    }
}

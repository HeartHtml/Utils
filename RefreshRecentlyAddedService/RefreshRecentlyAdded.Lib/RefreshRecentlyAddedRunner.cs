using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using RefreshRecentlyAddedDataStore;
using RefreshRecentlyAddedLogger;

namespace RefreshRecentlyAdded.Lib
{
    public class RefreshRecentlyAddedRunner
    {
        private readonly Logger _logger = new Logger();

        public RefreshRecentlyAddedArgs Args { get; set; }

        public RefreshRecentlyAddedDataStoreProvider DataStoreProvider { get; set; }

        [DllImport("kernel32.dll")]
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }

        protected void Create(RefreshRecentlyAddedArgs args)
        {
            Args = args;
            DataStoreProvider = new RefreshRecentlyAddedDataStoreProvider(args.DatabaseLocation,
                args.RandomFilePlayCountThresholdUntilDelete, args.MinimumPlayCount);
        }

        public RefreshRecentlyAddedRunner(RefreshRecentlyAddedArgs args)
        {
            Create(args);
        }

        public void RefreshFiles()
        {
            if (Args.Debug)
            {
                Thread.Sleep(Args.DebugWaitTime);
            }

            List<FileInfo> filesInScanLocation = GetScannableFiles(Args.ScanLocation).ToList();

            if(!Directory.Exists(Args.RandomPlaylistLocation))
            {
                Directory.CreateDirectory(Args.RandomPlaylistLocation);
            }

            if (!Directory.Exists(Args.RecentlyAddedLocation))
            {
                Directory.CreateDirectory(Args.RecentlyAddedLocation);
            }

            CleanDirectory(Args.RandomPlaylistLocation);

            CleanDirectory(Args.RecentlyAddedLocation);

            RefreshRandomFiles(filesInScanLocation,
                Args.RandomPlaylistLocation,
                Args.RandomPlaylistRefreshTimeIntervalInDays);

            RefreshRecentlyAddedFiles(filesInScanLocation,
                Args.RecentlyAddedLocation,
                Args.RefreshTimeIntervalInDays);

            DataStoreProvider.PersistDatabase();
        }

        private bool IsFileAllowed(string fileName)
        {
            foreach (string validMovieFileExtension in GetValidMovieFileExtensions())
            {
                if (fileName.ToLower().EndsWith(validMovieFileExtension.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        private List<string> GetValidMovieFileExtensions()
        {
            List<string> validMovieFileExtensions = new List<string>();
            string extensionsFromConfig = Args.ValidMovieExtensions;
            string[] extensionsSplit = extensionsFromConfig.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in extensionsSplit)
            {
                validMovieFileExtensions.Add(s);
            }
            return validMovieFileExtensions;
        }

        public void CleanDirectory(string directory)
        {
            try
            {
                List<FileInfo> files = GetFilesInLocation(directory, true).ToList();

                files.AsParallel().ForAll(dd => File.Delete(dd.FullName));
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }

        public void RefreshRecentlyAddedFiles(IEnumerable<FileInfo> fileInfos,
            string locationToCopyTo,
            int refreshTimeInterval)
        {
            try
            {
                List<FileInfo> filesInScanLocationList = fileInfos.OrderByDescending(dd => dd.CreationTime).ToList();

                List<FileInfo> filesInDestination = GetFilesInLocation(locationToCopyTo).ToList();

                int count = 1;

                foreach (FileInfo infoAboutFile in filesInScanLocationList)
                {
                    if (filesInDestination.FirstOrDefault(dd => dd.FullName.Contains(infoAboutFile.Name)) != null)
                    {
                        continue;
                    }

                    if (infoAboutFile.CreationTime >= DateTime.Today.AddDays(refreshTimeInterval * -1) &&
                        infoAboutFile.CreationTime <= DateTime.Today.AddDays(1).AddMilliseconds(-1))
                    {
                        string originalPath = infoAboutFile.FullName;

                        string newPath = Path.Combine(locationToCopyTo,
                            string.Format("{0}-{1}", count, infoAboutFile.Name));

                        CreateSymbolicLink(newPath, originalPath, SymbolicLink.File);

                        count++;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }

        protected IEnumerable<FileInfo> GetRandomFiles(int numberOfRandomFiles, IEnumerable<FileInfo> allFiles)
        {
            List<FileInfo> fileInfosToReturn = new List<FileInfo>();

            List<FileInfo> files = allFiles.ToList();

            bool readyToReturn = false;

            int iterationMax = 1000;

            int iterationCount = 0;

            while (!readyToReturn && iterationCount < iterationMax)
            {
                Random generator = new Random();

                List<int> indexes = new List<int>();

                for (int i = 0; i < numberOfRandomFiles; i++)
                {
                    int randomIndex = generator.Next(0, files.Count);

                    indexes.Add(randomIndex);
                }

                List<FileInfo> fileInfos = indexes.Select(t => files[t]).ToList();

                foreach (FileInfo info in fileInfos)
                {
                    if (DataStoreProvider.IsFileAllowed(info.FullName))
                    {
                        fileInfosToReturn.Add(info);
                    }

                    DataStoreProvider.IncreasePlayCount(info.FullName);

                    files.RemoveAll(dd => dd.FullName.Equals(info.FullName));

                    readyToReturn = fileInfosToReturn.Count == numberOfRandomFiles;

                    if (readyToReturn)
                    {
                        break;
                    }
                }

                iterationCount++;
            }

            return fileInfosToReturn;
        }

        public IEnumerable<FileInfo> GetFilesInLocation(string location, bool ignoreRestrictions = false)
        {
            List<FileInfo> files =
                Directory.EnumerateFiles(location, "*.*", SearchOption.AllDirectories)
                    .Where(dd => ignoreRestrictions || IsFileAllowed(dd))
                    .Select(dd => new FileInfo(dd))
                    .OrderBy(dd => dd.Name)
                    .ToList();

            return files;
        }

        public IEnumerable<FileInfo> GetScannableFiles(string scanLocation)
        {
            List<FileInfo> filesInScanLocationList = GetFilesInLocation(scanLocation).ToList();

            filesInScanLocationList.RemoveAll(
                ss => ss.DirectoryName != null && ss.DirectoryName.Contains(Args.RandomPlaylistLocation));

            filesInScanLocationList.RemoveAll(
                ss => ss.DirectoryName != null && ss.DirectoryName.Contains(Args.RecentlyAddedLocation));

            return filesInScanLocationList;
        }

        public void RefreshRandomFiles(IEnumerable<FileInfo> filesInScanLocation,
                                        string locationToCopyTo,
                                        int refreshTimeInterval)
        {
            try
            {
                List<FileInfo> filesInScanLocationList = filesInScanLocation.ToList();

                List<FileInfo> randomFiles = GetRandomFiles(Args.NumberOfRandomFiles, filesInScanLocationList).ToList();

                int count = 1;

                foreach (FileInfo randomFile in randomFiles)
                {
                    string originalPath = randomFile.FullName;

                    string newPath = Path.Combine(locationToCopyTo, string.Format("{0}-{1}", count, randomFile.Name));

                    CreateSymbolicLink(newPath, originalPath, SymbolicLink.File);

                    count++;
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }
    }
}
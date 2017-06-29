using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RefreshRecentlyAddedService;

namespace RefreshRecentlyAdded.Lib
{
    public class RefreshRecentlyAddedRunner
    {

        public string ValidMovieExtensions { get; set; }

        public bool Debug
        {
            get;
            set;
        }

        public int DebugWaitTime
        {
            get;
            set;
        }

        public bool RunRecentlyAddedRoutine
        {
            get;
            set;
        }

        public bool RunRandomPlaylistRoutine
        {
            get;
            set;
        }

        public string RandomOrgEndpoint
        {
            get;
            set;
        }

        public string ApiKey
        {
            get;
            set;
        }

        public int RandomPlaylistRefreshTimeIntervalInDays { get; set; }

        public int RefreshTimeIntervalInDays { get; set; }

        public int RefreshRateInSeconds { get; set; }

        public int NumberOfRandomFiles { get; set; }

        public string RecentlyAddedLocation { get; set; }

        public string ScanLocation { get; set; }

        public string RandomPlaylistLocation { get; set; }

        private Thread RefreshThread;

        [DllImport("kernel32.dll")]
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }

        public RefreshRecentlyAddedRunner()
        {

        }

        public RefreshRecentlyAddedRunner(int refreshTimeIntervalInDays,
                                            int refreshRateInSeconds,
                                            int randomPlaylistRefreshTimeIntervalInDays,
                                            int numberOfRandomFiles,
                                            string recentlyAddedLocation,
                                            string scanLocation,
                                            string randomPlaylistLocation,
                                            string validMovieExtensions,
                                            bool debug,
                                            int debugWaitTime,
                                            bool runRecentlyAddedRoutine,
                                            bool runRandomPlaylistRoutine,
                                            string randomOrgEndpoint,
                                            string apiKey)
        {
            RandomPlaylistRefreshTimeIntervalInDays = randomPlaylistRefreshTimeIntervalInDays;
            RefreshTimeIntervalInDays = refreshTimeIntervalInDays;
            RefreshRateInSeconds = refreshRateInSeconds;
            RecentlyAddedLocation = recentlyAddedLocation;
            ScanLocation = scanLocation;
            RandomPlaylistLocation = randomPlaylistLocation;
            NumberOfRandomFiles = numberOfRandomFiles;
            ValidMovieExtensions = validMovieExtensions;
            Debug = debug;
            DebugWaitTime = debugWaitTime;
            RunRecentlyAddedRoutine = runRecentlyAddedRoutine;
            RunRandomPlaylistRoutine = runRandomPlaylistRoutine;
            RandomOrgEndpoint = randomOrgEndpoint;
            ApiKey = apiKey;
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

        public void RefreshFiles()
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
        }

        private bool FileIsVideoFile(string Extension)
        {
            bool result = GetValidMovieFileExtensions().Contains(Extension);

            return result;
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
            string extensionsFromConfig = ValidMovieExtensions;
            string[] extensionsSplit = extensionsFromConfig.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in extensionsSplit)
            {
                validMovieFileExtensions.Add(s);
            }
            return validMovieFileExtensions;
        }

        public void CleanUpLocationFunction(string locationToCleanUp)
        {
            try
            {
                if (!Directory.Exists(locationToCleanUp))
                {
                    Directory.CreateDirectory(locationToCleanUp);
                }

                string[] files = Directory.GetFiles(locationToCleanUp, "*.*", SearchOption.AllDirectories);

                foreach (string currentFile in files)
                {
                    File.Delete(currentFile);
                }
            }
            catch (Exception e)
            {
                LogExceptionInEventViewer(e);
            }
        }

        public void RefreshLocationFunction(string locationOfSourceFiles,
                                                 string locationToCopyTo,
                                                 int refreshTimeInterval)
        {
            try
            {

                CleanUpLocationFunction(locationToCopyTo);

                if (!Directory.Exists(locationToCopyTo))
                {
                    Directory.CreateDirectory(locationToCopyTo);
                }

                string[] filesInScanLocation = Directory.GetFiles(locationOfSourceFiles, "*.*", SearchOption.AllDirectories);
                string[] filesInDestination = Directory.GetFiles(locationToCopyTo, "*.*", SearchOption.AllDirectories);

                List<String> filesInScanLocationList = filesInScanLocation.ToList();
                List<String> filesInDestinationList = GetFileNames(filesInDestination);

                filesInScanLocationList.RemoveAll(ss => ss.Contains(RandomPlaylistLocation));
                filesInScanLocationList.RemoveAll(ss => ss.Contains(RecentlyAddedLocation));

                List<FileInfo> fileInfos = filesInScanLocationList.Select(dd => new FileInfo(dd)).OrderByDescending(dd => dd.CreationTime).ToList();

                int count = 1;

                foreach (FileInfo infoAboutFile in fileInfos)
                {
                    if (!infoAboutFile.Name.ToLower().Contains(".DS_Store".ToLower()) &&
                        FileIsVideoFile(infoAboutFile.Extension))
                    {
                        if (filesInDestinationList.Contains(infoAboutFile.Name))
                        {
                            continue;
                        }

                        if (infoAboutFile.CreationTime >= DateTime.Today.AddDays(refreshTimeInterval * -1) &&
                            infoAboutFile.CreationTime <= DateTime.Today.AddDays(1).AddMilliseconds(-1))
                        {
                            string originalPath = infoAboutFile.FullName;

                            string newPath = Path.Combine(locationToCopyTo, string.Format("{0}-{1}", count, infoAboutFile.Name));

                            CreateSymbolicLink(newPath, originalPath, SymbolicLink.File);

                            count++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogExceptionInEventViewer(e);
            }
        }

        protected IEnumerable<FileInfo> GetRandomFiles(int numberOfRandomFiles, IEnumerable<FileInfo> allFiles)
        {
            List<FileInfo> files = allFiles.ToList();

            var httpClient = new HttpClient();

            var requestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(RandomOrgEndpoint),
                Method = new HttpMethod("POST"),
            };

            RandomNumberRequest request = new RandomNumberRequest
            {
                Id = 12546,
                Method = "generateIntegers",
                JsonRpc = "2.0",
                Params = new RandomNumberRequestParams
                {
                    ApiKey = ApiKey,
                    Base = 10,
                    Max = files.Count - 1,
                    Min = 0,
                    N = numberOfRandomFiles,
                    Replacement = true
                }
            };

            string requestContent = JsonConvert.SerializeObject(request);

            requestMessage.Content = new StringContent(requestContent);

            HttpResponseMessage responseMessage = Task.Run(() => httpClient.SendAsync(requestMessage)).Result;

            HttpContent content = responseMessage.Content;

            string randomOrgContent = Task.Run(() => content.ReadAsStringAsync()).Result;

            dynamic randomOrgResponse = JObject.Parse(randomOrgContent);

            JArray indexes = randomOrgResponse.result.random.data;

            return indexes.Select(t => files[t.Value<int>()]).ToList();
        }

        public void CreateRandomFilesInLocationFunction(string locationOfSourceFiles,
                                                        string locationToCopyTo,
                                                        int refreshTimeInterval)
        {
            try
            {
                CleanUpLocationFunction(locationToCopyTo);

                if (!Directory.Exists(locationToCopyTo))
                {
                    Directory.CreateDirectory(locationToCopyTo);
                }

                List<FileInfo> filesInScanLocationList = Directory.GetFiles(locationOfSourceFiles, "*.*", SearchOption.AllDirectories).Select(dd => new FileInfo(dd)).OrderBy(dd => dd.Name).ToList();

                filesInScanLocationList.RemoveAll(ss => ss.DirectoryName.Contains(RandomPlaylistLocation));

                filesInScanLocationList.RemoveAll(ss => ss.DirectoryName.Contains(RecentlyAddedLocation));

                filesInScanLocationList.RemoveAll(ss => !FileIsVideoFile(ss.Extension));

                List<FileInfo> randomFiles = GetRandomFiles(NumberOfRandomFiles, filesInScanLocationList).ToList();

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
                LogExceptionInEventViewer(e);
            }

        }

    }
}

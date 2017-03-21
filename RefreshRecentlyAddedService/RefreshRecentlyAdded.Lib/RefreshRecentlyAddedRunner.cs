using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
                    Max = files.Count,
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
                CleanUpLocationFunction(locationToCopyTo,
                                        refreshTimeInterval);

                if (!Directory.Exists(locationToCopyTo))
                {
                    Directory.CreateDirectory(locationToCopyTo);
                }

                List<FileInfo> filesInScanLocationList = Directory.GetFiles(locationOfSourceFiles, "*.*", SearchOption.AllDirectories).Select(dd => new FileInfo(dd)).ToList();

                filesInScanLocationList.RemoveAll(ss => ss.DirectoryName.Contains(RandomPlaylistLocation));

                filesInScanLocationList.RemoveAll(ss => ss.DirectoryName.Contains(RecentlyAddedLocation));

                filesInScanLocationList.RemoveAll(ss => !FileIsVideoFile(ss.Extension));

                List<FileInfo> randomFiles = GetRandomFiles(NumberOfRandomFiles, filesInScanLocationList).ToList();

                foreach (FileInfo randomFile in randomFiles)
                {
                    if (GetFileNames(Directory.GetFiles(locationToCopyTo, "*.*", SearchOption.AllDirectories)).Contains(randomFile.Name))
                    {
                        continue;
                    }

                    File.Copy(randomFile.FullName, Path.Combine(locationToCopyTo, randomFile.Name));
                }
            }
            catch (Exception e)
            {
                LogExceptionInEventViewer(e);
            }

        }

    }
}

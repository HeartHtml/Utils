using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefreshRecentlyAdded.Lib;

namespace RefreshRecentlyAddedLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] recentlyAddedLocations = Properties.Settings.Default.RecentlyAddedLocation.Split('|');
            string[] scanLocations = Properties.Settings.Default.ScanLocations.Split('|');
            string[] randomPlayListLocations = Properties.Settings.Default.RandomPlaylistLocation.Split('|');

            int refreshTimeIntervalInDays = Properties.Settings.Default.RefreshTimeIntervalInDays;
            int refreshRateInSeconds = Properties.Settings.Default.RefreshRateInSeconds;
            int randomPlayListRefreshTime = Properties.Settings.Default.RandomPlaylistRefreshTimeIntervalInDays;
            int numberOfRandomFiles = Properties.Settings.Default.NumberOfRandomFiles;

            string validMovieExtensions = Properties.Settings.Default.ValidMovieExtensions;
            bool debug = Properties.Settings.Default.Debug;
            int debugWaitTime = Properties.Settings.Default.DebugWaitTime;
            bool runRecentlyAddedRoutine = Properties.Settings.Default.RunRecentlyAddedRoutine;
            bool runRandomPlaylistRoutine = Properties.Settings.Default.RunRandomPlaylistRoutine;
            string randomOrgEndpoint = Properties.Settings.Default.RandomOrgEndpoint;
            string apiKey = Properties.Settings.Default.ApiKey;

            int minimumPlayCount = Properties.Settings.Default.MinimumPlayCount;

            int randomPlayCountUntilDelete = Properties.Settings.Default.RandomFilePlayCountThresholdUntilDelete;

            string dbPath = Properties.Settings.Default.DatabaseFilePath;

            RefreshRecentlyAddedRunner refresher = new RefreshRecentlyAddedRunner(
                    refreshTimeIntervalInDays,
                    refreshRateInSeconds,
                    randomPlayListRefreshTime,
                    numberOfRandomFiles,
                    recentlyAddedLocations[0],
                    scanLocations[0],
                    randomPlayListLocations[0],
                    validMovieExtensions,
                    debug,
                    debugWaitTime,
                    runRecentlyAddedRoutine,
                    runRandomPlaylistRoutine,
                    randomOrgEndpoint,
                    apiKey,
                    minimumPlayCount,
                    randomPlayCountUntilDelete,
                    dbPath
                    );

            refresher.RefreshFiles();
        }
    }
}

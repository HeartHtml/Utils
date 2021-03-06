﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using RefreshRecentlyAdded.Lib;

namespace RefreshRecentlyAddedService
{
    public partial class RecentlyAddedService : ServiceBase
    {
        List<RefreshRecentlyAddedRunner> refreshers = new List<RefreshRecentlyAddedRunner>();

        public RecentlyAddedService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            refreshers = GetRefreshersFromAppConfig();
            foreach (RefreshRecentlyAddedRunner refresher in refreshers)
            {
                refresher.StartRefreshing();
            }
        }

        protected override void OnStop()
        {
            if (refreshers != null)
            {
                foreach (RefreshRecentlyAddedRunner refresher in refreshers)
                {
                    refresher.StopRefreshing();
                }
            }
        }

        private List<RefreshRecentlyAddedRunner> GetRefreshersFromAppConfig()
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


            List<RefreshRecentlyAddedRunner> refresh = new List<RefreshRecentlyAddedRunner>();

            for (int i = 0; i < scanLocations.Length; i++)
            {
                RefreshRecentlyAddedRunner refresher = new RefreshRecentlyAddedRunner(
                    refreshTimeIntervalInDays,
                    refreshRateInSeconds,
                    randomPlayListRefreshTime,
                    numberOfRandomFiles,
                    recentlyAddedLocations[i],
                    scanLocations[i],
                    randomPlayListLocations[i],
                    validMovieExtensions,
                    debug,
                    debugWaitTime,
                    runRecentlyAddedRoutine,
                    runRandomPlaylistRoutine,
                    randomOrgEndpoint,
                    apiKey
                    );

                refresh.Add(refresher);
            }
            return refresh;
        }
    }
}

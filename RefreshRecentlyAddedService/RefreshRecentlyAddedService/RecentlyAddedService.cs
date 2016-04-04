using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace RefreshRecentlyAddedService
{
    public partial class RecentlyAddedService : ServiceBase
    {
        List<RefreshRecentlyAdded> refreshers = new List<RefreshRecentlyAdded>();

        public RecentlyAddedService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            refreshers = GetRefreshersFromAppConfig();
            foreach (RefreshRecentlyAdded refresher in refreshers)
            {
                refresher.StartRefreshing();
            }
        }

        protected override void OnStop()
        {
            if (refreshers != null)
            {
                foreach (RefreshRecentlyAdded refresher in refreshers)
                {
                    refresher.StopRefreshing();
                }
            }
        }

        private List<RefreshRecentlyAdded> GetRefreshersFromAppConfig()
        {
            string[] recentlyAddedLocations = Properties.Settings.Default.RecentlyAddedLocation.Split('|');
            string[] scanLocations = Properties.Settings.Default.ScanLocations.Split('|');
            string[] randomPlayListLocations = Properties.Settings.Default.RandomPlaylistLocation.Split('|');

            int refreshTimeIntervalInDays = Properties.Settings.Default.RefreshTimeIntervalInDays;
            int refreshRateInSeconds = Properties.Settings.Default.RefreshRateInSeconds;
            int randomPlayListRefreshTime = Properties.Settings.Default.RandomPlaylistRefreshTimeIntervalInDays;
            int numberOfRandomFiles = Properties.Settings.Default.NumberOfRandomFiles;

            List<RefreshRecentlyAdded> refresh = new List<RefreshRecentlyAdded>();

            for (int i = 0; i < scanLocations.Length; i++)
            {
                RefreshRecentlyAdded refresher = new RefreshRecentlyAdded(
                    refreshTimeIntervalInDays,
                    refreshRateInSeconds,
                    randomPlayListRefreshTime,
                    numberOfRandomFiles,
                    recentlyAddedLocations[i],
                    scanLocations[i],
                    randomPlayListLocations[i]
                    );

                refresh.Add(refresher);
            }
            return refresh;
        }
    }
}

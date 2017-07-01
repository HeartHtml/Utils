using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefreshRecentlyAdded.Lib
{
    public class RefreshRecentlyAddedArgs
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

        public int RandomPlaylistRefreshTimeIntervalInDays { get; set; }

        public int RefreshTimeIntervalInDays { get; set; }

        public int NumberOfRandomFiles { get; set; }

        public string RecentlyAddedLocation { get; set; }

        public string ScanLocation { get; set; }

        public string RandomPlaylistLocation { get; set; }

        public string DatabaseLocation { get; set; }

        public int RandomFilePlayCountThresholdUntilDelete { get; set; }

        public int MinimumPlayCount { get; set; }
    }
}

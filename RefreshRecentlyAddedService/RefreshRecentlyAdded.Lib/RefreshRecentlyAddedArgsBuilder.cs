using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefreshRecentlyAdded.Lib
{
    public class RefreshRecentlyAddedArgsBuilder
    {
        public RefreshRecentlyAddedArgs Build(KeyValueConfigurationCollection collection)
        {
            RefreshRecentlyAddedArgs refreshArgs = new RefreshRecentlyAddedArgs
            {
                Debug = Convert.ToBoolean(collection["Debug"].Value),
                MinimumPlayCount = Convert.ToInt32(collection["MinimumPlayCount"].Value),
                RandomFilePlayCountThresholdUntilDelete = Convert.ToInt32(collection["RandomFilePlayCountThresholdUntilDelete"].Value),
                DatabaseLocation = collection["DatabaseLocation"].Value,
                DebugWaitTime = Convert.ToInt32(collection["DebugWaitTime"].Value),
                NumberOfRandomFiles = Convert.ToInt32(collection["NumberOfRandomFiles"].Value),
                RandomPlaylistLocation = collection["RandomPlaylistLocation"].Value,
                RandomPlaylistRefreshTimeIntervalInDays = Convert.ToInt32(collection["RandomPlaylistRefreshTimeIntervalInDays"].Value),
                RecentlyAddedLocation = collection["RecentlyAddedLocation"].Value,
                RefreshTimeIntervalInDays = Convert.ToInt32(collection["RefreshTimeIntervalInDays"].Value),
                ScanLocation = collection["ScanLocation"].Value,
                ValidMovieExtensions = collection["ValidMovieExtensions"].Value,
            };

            return refreshArgs;
        }
    }
}

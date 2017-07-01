using System;
using System.Collections.Generic;
using System.Configuration;
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
            RefreshRecentlyAddedArgsBuilder builder = new RefreshRecentlyAddedArgsBuilder();

            Configuration config =
               ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            RefreshRecentlyAddedArgs refreshArgs = builder.Build(config.AppSettings.Settings);

            RefreshRecentlyAddedRunner refresher = new RefreshRecentlyAddedRunner(refreshArgs);

            refresher.RefreshFiles();
        }
    }
}

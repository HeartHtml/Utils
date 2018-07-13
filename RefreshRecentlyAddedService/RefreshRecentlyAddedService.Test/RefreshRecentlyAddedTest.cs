using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefreshRecentlyAdded.Lib;

namespace RefreshRecentlyAddedService.Test
{
    [TestClass]
    public class RefreshRecentlyAddedTest
    {
        [TestMethod]
        public void TestRandomFileGenerator()
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

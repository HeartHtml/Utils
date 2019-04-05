using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RefreshRecentlyAdded.Lib;
using RefreshRecentlyAddedLogger;

namespace RRALauncher
{
    public partial class _Default : Page
    {
        private readonly Logger _logger = new Logger();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLaunchApp_OnClick(object sender, EventArgs e)
        {
            string configPath = ConfigurationManager.AppSettings["LauncherConfigPath"];

            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = configPath
            };

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            try
            {
                RefreshRecentlyAddedArgsBuilder builder = new RefreshRecentlyAddedArgsBuilder();

                RefreshRecentlyAddedArgs args = builder.Build(config.AppSettings.Settings);

                RefreshRecentlyAddedRunner runner = new RefreshRecentlyAddedRunner(args);

                runner.RefreshFiles();
            }
            catch (Exception exception)
            {
                _logger.LogException(exception);
            }
            
        }
    }
}
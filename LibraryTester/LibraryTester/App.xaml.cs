using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;

namespace LibraryTester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public List<string> AssembliesCopied { get; set; } 

        private void App_OnExit(object sender, ExitEventArgs e)
        {

        }
    }
}

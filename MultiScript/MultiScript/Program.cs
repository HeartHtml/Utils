using System;
using System.IO;
using System.Windows.Forms;
using JS.Entities.ExtensionMethods;
using MultiScript.Forms;

namespace MultiScript
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            string folderToUse = string.Empty;

            string[] args = Environment.GetCommandLineArgs() ;
            if (args.SafeAny())
            {
                try
                {
                    string argument = args[1];
                    if (!argument.IsNullOrWhiteSpace())
                    {
                        if (Directory.Exists(argument))
                        {
                            folderToUse = argument;
                        }
                    }
                }
                catch (Exception) { }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mainForm = new MainForm();

            if (!folderToUse.IsNullOrWhiteSpace())
            {
                mainForm.SetFolder(folderToUse);
            }

            Application.Run(mainForm);
        }
    }
}

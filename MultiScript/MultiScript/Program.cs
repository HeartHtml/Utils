using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MultiScript.Forms;
using UtilsLib.ExtensionMethods;

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
            if (args.Any())
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

            if (!string.IsNullOrWhiteSpace(folderToUse))
            {
                mainForm.SetFolder(folderToUse);
            }

            Application.Run(mainForm);
        }
    }
}

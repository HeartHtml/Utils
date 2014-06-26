using System;
using System.Threading;
using MultiScriptLib;
using UtilsLib.ExtensionMethods;

namespace MultiScriptCL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "MultiScript";
            if (args.Length > 1)
            {
                CommandLineArguments commandLineArguments = new CommandLineArguments();

                for (int i = 0; i < args.Length; i++)
                {
                    bool hasNextArgument = i != args.Length - 1;
                    string currentArg = args[i];

                    switch (currentArg.ToLower())
                    {
                        case "-db":
                        case "-database":
                        {
                            if (hasNextArgument)
                            {
                                commandLineArguments.DatabaseName = args[i + 1].TrimSafely();
                            }
                            break;
                        }

                        case "-server":
                        {
                            if (hasNextArgument)
                            {
                                commandLineArguments.ServerName = args[i + 1].TrimSafely();
                            }
                            break;
                        }
                        case "-conn":
                        case "-connectionstring":
                        {
                            if (hasNextArgument)
                            {
                                commandLineArguments.ConnectionString = args[i + 1].TrimSafely();

                                commandLineArguments.OverrideConnectionString = true;
                            }
                            break;
                        }

                        case "-s":
                        case "-scriptfolder":
                        {
                            if (hasNextArgument)
                            {
                                commandLineArguments.MainFolderToRun = args[i + 1].TrimSafely();
                            }
                            break;
                        }
                        case "-addsubfolders":
                        {
                            commandLineArguments.AddAllSubfolders = true;
                            break;
                        }
                        case "-h":
                        case "-help":
                            DisplayHelp();
                            return;
                    }
                }

                if (commandLineArguments.IsValid)
                {
                    if (commandLineArguments.DatabaseName.IsNullOrWhiteSpace())
                    {
                        commandLineArguments.DatabaseName = Properties.Settings.Default.DefaultDatabase;
                    }

                    if (!commandLineArguments.OverrideConnectionString)
                    {
                        bool isProduction = commandLineArguments.EnvironmentContext == EnumEnvironments.Production;

                        commandLineArguments.ConnectionString =
                            MultiScript.GetConnectionString(commandLineArguments.EnvironmentContext,
                                commandLineArguments.Database, isProduction);
                    }

                    string errorMessage;

                    if (commandLineArguments.BuildFoldersToRun(out errorMessage))
                    {
                        MultiScript.RunMultiScriptCommandLine(commandLineArguments);
                    }
                    else
                    {
                        Console.WriteLine("PROBLEMS WITH FOLDERS OR FOLDER STRUCTURE");
                        Console.WriteLine();
                        Console.WriteLine(errorMessage);
                        Environment.ExitCode = 1;
                    }
                }
                else
                {
                    Console.WriteLine("INVALID ARGUMENTS");
                    Console.WriteLine();
                    Console.WriteLine(commandLineArguments.CommandLineInvalidArgumentDetails);
                    DisplayHelp();
                    Environment.ExitCode = 1;
                }
            }
            else
            {
                DisplayHelp();
            }
        }

        public static void DisplayHelp()
        {
            Console.WriteLine();
            Console.WriteLine("=====================================================================");
            Console.WriteLine("MultiScript Command Line Help");
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            Console.WriteLine("\t-h/--help : Displays this help guide.");
            Console.WriteLine("\t-s/-scriptFolder : Main folder run MultiScript on.");
            Console.WriteLine("\t-server : Server to run MultiScript on.");
            Console.WriteLine("\t-db/-database : (Optional) Database to run MultiScript on.");
            Console.WriteLine(
                "\t-conn/-connectionstring : (Optional) If provided will use this connection string explicitly.");
            Console.WriteLine("=====================================================================");
            Console.WriteLine();
        }
    }
}
    ;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace CleanDirectory
{
    class Program
    {
        public enum EnumExecutionMode
        {
            None = 0,
            RecursiveDeleteAll = 1,
            IgnoreSpecifiedDirectory = 2
        }

        public static bool AnyError { get; set; }

        public static EnumExecutionMode ExecutionMode { get; set; }

        public static void Main(string[] args)
        {
            try
            {
                AnyError = false;

                if (args.Length == 0)
                {
                    Console.WriteLine("Missing required argument: Directory to clean!");
                }
                else
                {
                    string directoryToClean = args[0];

                    if (!Directory.Exists(directoryToClean))
                    {
                        Console.WriteLine("Directory to clean does not exist");

                        Environment.Exit(0);
                    }

                    ExecutionMode = EnumExecutionMode.RecursiveDeleteAll;

                    List<string> subDirectoriesToIgnore = new List<string>();

                    if (args.Length >= 3)
                    {
                        if (args[1] != "/i" && string.IsNullOrWhiteSpace(args[2]))
                        {
                            Console.WriteLine("Invalid arguments specified");

                            Environment.Exit(0);
                        }
                        else
                        {
                            ExecutionMode = EnumExecutionMode.IgnoreSpecifiedDirectory;

                            List<string> ignoreFolderNames = args[2].Split('|').ToList();

                            subDirectoriesToIgnore = ignoreFolderNames.Select(s => Path.Combine(directoryToClean, s)).ToList();

                            foreach (string subDirectoryToIgnore in subDirectoriesToIgnore)
                            {
                                if (!Directory.Exists(subDirectoryToIgnore))
                                {
                                    Console.WriteLine("Ignore directory does not exist.");

                                    Environment.Exit(0);
                                }
                            }
                        }
                    }

                    try
                    {
                        CleanDirectoryAndSubfolders(directoryToClean, subDirectoriesToIgnore, ExecutionMode, true, true);

                        Console.WriteLine("FIN");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        LogException(ex);
                        AnyError = true;
                    }

                    Environment.ExitCode = AnyError ? 1 : 0;

                    if (Environment.ExitCode != 0)
                    {
                        Console.WriteLine("Some errors were encountered :(");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Some errors were encountered :(");
                LogException(ex);  
                Environment.Exit(1);
            }
        }

        public static void CleanDirectoryAndSubfolders(string root, List<string> ignoreDirectories = null, EnumExecutionMode executionMode = EnumExecutionMode.RecursiveDeleteAll, bool listFilesDeleted = true, bool isTopmostRoot = false)
        {
            string[] files = Directory.GetFiles(root);

            foreach (string file in files)
            {
                try
                {
                    if (listFilesDeleted)
                    {
                        Console.WriteLine("Deleting file: {0}", file);
                    }

                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    AnyError = true;
                }
            }

            List<string> subDirectories = Directory.GetDirectories(root).ToList();

            if (executionMode == EnumExecutionMode.IgnoreSpecifiedDirectory)
            {
                if (ignoreDirectories != null)
                {
                    foreach (string ignoreDirectory in ignoreDirectories)
                    {
                        subDirectories.Remove(ignoreDirectory);
                    }
                }
            }

            foreach (string subDirectory in subDirectories)
            {
                CleanDirectoryAndSubfolders(subDirectory);
            }

            //At this point the directory should be clean of any files, delete it if it's not the root
            if (!isTopmostRoot)
            {
                try
                {
                    if (listFilesDeleted)
                    {
                        Console.WriteLine("Deleting directory: {0}", root);
                    }

                    Directory.Delete(root);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    AnyError = true;
                }
            }
        }

        private static void LogException(Exception e)
        {
            if (!EventLog.SourceExists("Clean Directory"))
            {
                EventLog.CreateEventSource("Clean Directory", "Application");
            }

            EventLog.WriteEntry("Clean Directory", string.Format("Unhandled Exception:\nTimestamp: {0}\nStack Track: {1}\nMessage: {2}", DateTime.Now, e.StackTrace, e.Message), EventLogEntryType.Error);
        }
    }
}

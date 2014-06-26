using System;
using System.IO;
using System.Threading;
using Ionic.Zip;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] commandLineArgs = Environment.GetCommandLineArgs();
        if (commandLineArgs.Length == 0 || commandLineArgs.Length == 1)
        {
            Console.WriteLine("Missing Required Parameter: {0}", "Directory");
            Environment.Exit(1);
        }

        string text = commandLineArgs[1];
        string text2 = null;
        if (commandLineArgs.Length > 2)
        {
            text2 = commandLineArgs[2];
        }
        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("Missing Required Parameter: {0}", "Directory");
            Environment.Exit(1);
        }
        if (!Directory.Exists(text))
        {
            Console.WriteLine("Directory {0} does not exist", text);
            Environment.Exit(0);
        }
        if (string.IsNullOrWhiteSpace(text2))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(text);
            text2 = ((directoryInfo.Parent == null) ? directoryInfo.Root.FullName : directoryInfo.Parent.FullName);
            Console.WriteLine("Destination directory not specified. Will use path of parent directory of directory to archive: {0}", text2);
        }
        if (!Directory.Exists(text2))
        {
            Console.WriteLine("Destination directory does not exist. Creating destination directory");
            Directory.CreateDirectory(text2);
        }
        if (ArchiveAndDateDirectory(text, text2))
        {
            Console.WriteLine("FIN");
            Environment.Exit(0);
        }
        else
        {
            Environment.Exit(1);
        }
    }
    private static bool ArchiveAndDateDirectory(string directory, string destinationDirectory)
    {
        bool result;
        try
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            string path = string.Format("{0}.{1}.{2}.{3}.zip", new object[]
            {
                DateTime.Today.Year,
                DateTime.Today.Month,
                DateTime.Today.Day,
                directoryInfo.Name
            });
            string text = Path.Combine(destinationDirectory, path);

            string originalName = text;

            if (File.Exists(text))
            {
                try
                {
                    int counter = 1;

                    text = Path.Combine(destinationDirectory, string.Format("{0}.{1}.{2}.{3}.{4}.zip", new object[]
                                    {
                                        DateTime.Today.Year,
                                        DateTime.Today.Month,
                                        DateTime.Today.Day,
                                        directoryInfo.Name,
                                        counter
                                    }));

                    while (File.Exists(text))
                    {
                        counter ++;

                        text = Path.Combine(destinationDirectory, string.Format("{0}.{1}.{2}.{3}.{4}.zip", new object[]
                                    {
                                        DateTime.Today.Year,
                                        DateTime.Today.Month,
                                        DateTime.Today.Day,
                                        directoryInfo.Name,
                                        counter
                                    }));
                    }

                    Console.WriteLine("Archive with name {0} found. Renaming file to new name {1}.", originalName, text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Deleting existing archive failed. Will try to proceed anyway.");
                }
            }
            using (ZipFile zipFile = new ZipFile())
            {
                zipFile.AddDirectory(directory, directoryInfo.Name);
                zipFile.Save(text);
            }
            FileInfo fileInfo = new FileInfo(text);
            Console.WriteLine("Archive {0} created", fileInfo.FullName);
            result = true;
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        result = false;
        return result;
    }
}
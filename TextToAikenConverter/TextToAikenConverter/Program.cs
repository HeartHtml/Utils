using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NDesk.Options;
using UtilsLib.ExtensionMethods;

namespace TextToAikenConverter
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (Properties.Settings.Default.IsDebug)
            {
                Thread.Sleep(20000);
            }

            bool showHelp = false;

            string sourceQuizFilePath = string.Empty;

            OptionSet p = new OptionSet
            {
                {"SourceQuizPath=", "The path of the source file.", v => sourceQuizFilePath = v},
                {"h|help", "Show this message and exit", v => showHelp = v != null },
            };

            List<string> extra;

            try
            {
                extra = p.Parse(args);
            }
            catch (Exception ex)
            {
                Console.Write("Text To Aiken Converter: ");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try `TextToAikenConverter --help' for more information.");
                return;
            }

            if (showHelp || extra.Count > 0 || args.Length == 0)
            {
                ShowHelp(p);
                return;
            }

            try
            {
                AikenConverterHelper helper = new AikenConverterHelper(sourceQuizFilePath);

                FileInfo info = new FileInfo(sourceQuizFilePath);

                string originalFileName = info.Name.Replace(".txt", string.Empty);

                string newName = string.Format("{0}_Aiken.txt", originalFileName);

                string newPath;

                if (string.IsNullOrWhiteSpace(info.DirectoryName))
                {
                    newPath = Path.Combine(Environment.SystemDirectory, newName);
                }
                else {
                    newPath = Path.Combine(info.DirectoryName, newName);
                }

                File.WriteAllText(newPath, helper.GetQuizInRawAikenFormat());

                Console.WriteLine("File processed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: TextToAikenConverter [OPTIONS]+");
            Console.WriteLine("Create an Aiken formatted quiz file from a source file.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}

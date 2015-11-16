using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk.Options;
using UtilsLib.ExtensionMethods;

namespace KbsSigner
{
    class Program
    {
        public static void Main(string[] args)
        {
            string kbsPath = string.Empty;

            string metaPath = string.Empty;

            bool open = false;

            bool showHelp = false;

            OptionSet p = new OptionSet
            {
                {"kbs=", "The path of the kbs formatted file.", v => kbsPath = v},
                {"meta=", "(Optional) The path of the kbs meta data file.", v => metaPath = v},
                {"open", "(Optional) Flag indicating whether to open the PDF file after creation.", v => open = !string.IsNullOrWhiteSpace(v)},
                {"h|help", "Show this message and exit", v => showHelp = v != null },
            };

            List<string> extra;

            try
            {
                extra = p.Parse(args);
            }
            catch (Exception ex)
            {
                Console.Write("KbsSigner: ");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try `KbsSigner --help' for more information.");
                return;
            }

            if (showHelp || extra.Count > 0 || args.Length == 0)
            {
                ShowHelp(p);
                return;
            }

            int exitCode = 0;

            try
            {
                if (metaPath.IsNullOrWhiteSpace())
                {
                    Console.WriteLine("Meta data file not supplied. Will not generate pdf with meta data.");
                }

                KbsJob job = new KbsJob(kbsPath, metaPath);

                if (!job.MetaData.ParseErrorMessage.IsNullOrWhiteSpace())
                {
                    Console.WriteLine("Errors encountered parsing meta data file:");
                    Console.WriteLine(job.MetaData.ParseErrorMessage);
                }

                KbsSignerWorker worker = new KbsSignerWorker(job);
                
                worker.GenerateKbsSignedPdf();

                Console.WriteLine("Success! PDF file generated at {0}", job.DestinationFilePath);

                if (open)
                {
                    Process.Start(job.DestinationFilePath);
                }

            }
            catch (Exception ex)
            {
                exitCode = 1;
                Console.WriteLine("Errors were encountered:");
                Console.WriteLine(ex.Message);
            }

            Environment.Exit(exitCode);

        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: KbsSigner [OPTIONS]+");
            Console.WriteLine("Creates a PDF file from a kbs formatted text file.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}

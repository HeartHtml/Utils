using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk.Options;
using UtilsLib.ExtensionMethods;

namespace TrussSigner
{
    class Program
    {
        public static void Main(string[] args)
        {
            string trussPath = string.Empty;

            string metaPath = string.Empty;

            bool open = false;

            bool showHelp = false;

            OptionSet p = new OptionSet
            {
                {"truss=", "The path of the truss formatted file (kbs, pcl file).", v => trussPath = v},
                {"meta=", "(Optional) The path of the meta data file.", v => metaPath = v},
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
                Console.Write("TrussSigner: ");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try `TrussSigner --help' for more information.");
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

                TrussJob job = new TrussJob(trussPath, metaPath);

                if (!job.MetaData.ParseErrorMessage.IsNullOrWhiteSpace())
                {
                    Console.WriteLine("Errors encountered parsing meta data file:");
                    Console.WriteLine(job.MetaData.ParseErrorMessage);
                }

                TrussSignerWorker worker = new TrussSignerWorker(job);
                
                worker.GenerateTrussSignedPdf();

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
            Console.WriteLine("Usage: TrussSigner [OPTIONS]+");
            Console.WriteLine("Creates a PDF file from a truss formatted text file.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}

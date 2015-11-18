using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk.Options;

namespace BillOfLadingParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string bol = string.Empty;

            string metaPath = string.Empty;

            bool open = false;

            bool showHelp = false;

            OptionSet p = new OptionSet
            {
                {"bol=", "The path of the kbs formatted file.", v => bol = v},
                {"open", "(Optional) Flag indicating whether to open the file after creation.", v => open = !string.IsNullOrWhiteSpace(v)},
                {"h|help", "Show this message and exit", v => showHelp = v != null },
            };

            List<string> extra;

            try
            {
                extra = p.Parse(args);
            }
            catch (Exception ex)
            {
                Console.Write("BillOfLadingParser: ");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try `BillOfLadingParser --help' for more information.");
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
                BillOfLadingRequest request = new BillOfLadingRequest(bol);

                BillOfLadingWorker worker = new BillOfLadingWorker(request);

                BillOfLadingResponse response = worker.GenerateBillOfLadingMetaFile();

                Console.WriteLine("Success! Parsed file generated at {0}", response.BillOfLadingMetaFilePath);

                if (open)
                {
                    Process.Start(response.BillOfLadingMetaFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Environment.Exit(exitCode);
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: BillOfLadingParser [OPTIONS]+");
            Console.WriteLine("Parses a Bill Of Lading Pdf file into a delimited text file.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}

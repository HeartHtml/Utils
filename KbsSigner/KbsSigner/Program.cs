using System;
using System.Collections.Generic;
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

            bool showHelp = false;

            OptionSet p = new OptionSet
            {
                {"kbs=", "The path of the kbs formatted file.", v => kbsPath = v},
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
                KbsSignerWorker worker = new KbsSignerWorker(kbsPath);

                FileInfo info = new FileInfo(kbsPath);

                string directory = Environment.SystemDirectory;

                if (info.DirectoryName != null)
                {
                    directory = info.DirectoryName;
                }

                string newPath = Path.Combine(directory, string.Format("{0}.pdf", info.Name.Split(".")[0]));

                worker.GenerateKbsSignedPdf(newPath);

                Console.WriteLine("Success! PDF file generated at {0}", newPath);

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

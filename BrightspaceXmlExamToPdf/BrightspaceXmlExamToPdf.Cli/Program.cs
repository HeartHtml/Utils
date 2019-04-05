using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightspaceXmlExamToPdf.Lib;
using NDesk.Options;
using UtilsLib.ExtensionMethods;

namespace BrightspaceXmlExamToPdf.Cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string groupXmlPath = string.Empty;

            string pdfOutputPath = string.Empty;

            bool showHelp = false;

            OptionSet p = new OptionSet
            {
                {"e=", "The path of the group xml exam file.", v => groupXmlPath = v},
                {"p=", "The path of the pdf file to output. (Optional)", v => pdfOutputPath = v},
                {"h|help", "Show this message and exit", v => showHelp = v != null },
            };

            List<string> extra;

            try
            {
                extra = p.Parse(args);
            }
            catch (Exception ex)
            {
                Console.Write("BrightspaceXmlExamToPdf: ");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try `BrightspaceXmlExamToPdf --help' for more information.");
                return;
            }

            if (showHelp || extra.Count > 0 || args.Length == 0)
            {
                ShowHelp(p);
                return;
            }

            if (groupXmlPath.IsNullOrWhiteSpace())
            {
                ShowHelp(p);
                return;
            }

            if (pdfOutputPath.IsNullOrWhiteSpace())
            {
                FileInfo info = new FileInfo(groupXmlPath);

                pdfOutputPath = Path.Combine(Directory.GetCurrentDirectory(), info.Name + ".pdf");
            }

            int exitCode = 0;

            try
            {
                BrightspaceXmlExamContext context = new BrightspaceXmlExamContext(groupXmlPath);

                BrightspaceXmlExamPdfConverter pdfConverter = new BrightspaceXmlExamPdfConverter(context);

                pdfConverter.CreatePdfFromContext(pdfOutputPath);

                Console.WriteLine("Pdf file created.");
            }
            catch (Exception e)
            {
                exitCode = 1;
                Console.WriteLine("Errors were encountered:");
                Console.WriteLine(e.Message);
            }

            Environment.Exit(exitCode);

        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: BrightspaceXmlExamToPdf [OPTIONS]+");
            Console.WriteLine("Creates a PDF file from a group xml exam file.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using UtilsLib.ExtensionMethods;

namespace BillOfLadingParser
{
    public class BillOfLadingWorker
    {
        public BillOfLadingRequest Request { get; set; }

        public BillOfLadingWorker(BillOfLadingRequest request)
        {
            Request = request;
        }

        public BillOfLadingResponse GenerateBillOfLadingMetaFile()
        {
            if (Request == null)
            {
                throw new NoNullAllowedException("Request was null");
            }

            if (Request.BillOfLadingFilePath.IsNullOrWhiteSpace())
            {
                throw new NoNullAllowedException("Bill of lading file path was null");
            }

            if (!File.Exists(Request.BillOfLadingFilePath))
            {
                throw new FileNotFoundException("Bill of lading file not found");
            }

            BillOfLadingResponse response = ParseBillOfLadingFile();

            CreateBillOfLadingMetaFile(response);

            return response;
        }

        private void CreateBillOfLadingMetaFile(BillOfLadingResponse response)
        {
            File.WriteAllText(response.BillOfLadingMetaFilePath, response.ToString());
        }

        private BillOfLadingResponse ParseBillOfLadingFile()
        {
            BillOfLadingResponse response = new BillOfLadingResponse();

            FileInfo info = new FileInfo(Request.BillOfLadingFilePath);

            string metaFilePath = info.FullName.Replace(info.Extension, ".txt");

            response.BillOfLadingMetaFilePath = metaFilePath;

            PdfReader pdfReader = new PdfReader(Request.BillOfLadingFilePath);

            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

                string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                currentText = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));

                string[] lines = currentText.Split("\n");

                BillOfLadingMetaFileLine line = null;

                foreach (string splitLine in lines)
                {
                    if (splitLine.Contains("ID:"))
                    {
                        if (line != null)
                        {
                            response.MetaFileLines.Add(line);
                        }

                        line = new BillOfLadingMetaFileLine();

                        string[] splitIdString = splitLine.Split("ID:");

                        line.TrussId = splitIdString[0];
                    }
                    //This is an at line
                    else if (splitLine.Contains("Rmax") && splitLine.Contains("At"))
                    {
                        string[] splitAtLine = splitLine.Split("At");

                        foreach (string atLine in splitAtLine)
                        {
                            string reconstructedAt = string.Format("At {0}", atLine);

                            if (line != null)
                            {
                                line.AtList.Add(reconstructedAt);
                            }
                        }
                    }
                }

                if (line != null &&
                    !line.TrussId.IsNullOrWhiteSpace())
                {
                    response.MetaFileLines.Add(line); 
                }
            }

            return response;
        }
    }
}

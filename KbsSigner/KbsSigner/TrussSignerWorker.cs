using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using UtilsLib.ExtensionMethods;

namespace TrussSigner
{

    public class TrussSignerWorker
    {
        private int MaxTrussCounter
        {
            get
            {
                return 120;
            }
        }

        private int InitialTrussCounter
        {
            get
            {
                return 110;
            }
        }

        public TrussJob TrussJob { get; set; }

        public TrussSignerWorker(TrussJob job)
        {
            TrussJob = job;
        }

        public TrussSignerWorker()
        {
            
        }

        private int GetMaxTrussByPage(int currentPage)
        {
            return currentPage == 0 ? InitialTrussCounter : MaxTrussCounter;
        }

        private Rectangle DetermineDocumentSize()
        {
            byte[] pdfContent = GetPdfTemplate(1);

            using (PdfReader reader = new PdfReader(pdfContent))
            {
                Rectangle size = reader.GetPageSizeWithRotation(1);

                return size;
            }
        }

        private int GetPadAmount(int column)
        {
            switch (column)
            {
                case 0:
                {
                    return 0;
                }
                case 1:
                {
                    return 7;
                }
                case 2:
                {
                    return 10;
                }
                case 3:
                {
                    return 13;
                }
                case 4:
                {
                    return 18;
                }
                case 5:
                {
                    return 23;
                }
                default:
                {
                    return 0;
                }
            }
        }

        private TrussPoint DetermineLineLocation(int index, int pageIndex)
        {
            TrussPoint point = new TrussPoint();

            decimal startingX = 50m;

            const decimal startingY = 387m;

            const decimal baseXAmount = 91m;

            const decimal baseYAmount = 13.68m;

            int destinationColumn = Convert.ToInt32(Math.Floor(Convert.ToDecimal(index)/20m));

            if (pageIndex == 0 && index > 94)
            {
                destinationColumn = 5;

                index = index + 5;
            }

            int pad = GetPadAmount(destinationColumn);

            if (destinationColumn > 0)
            {
                startingX += pad;
            }

            decimal destinationXAmount = startingX + (baseXAmount * destinationColumn);

            int distanceFromNextWholeDivider = index;

            int increments = 0;

            while (distanceFromNextWholeDivider % 20 != 0)
            {
                distanceFromNextWholeDivider --;

                increments ++;
            }

            int positionInColumn = increments;

            decimal destinationYAmount = startingY - (baseYAmount * positionInColumn);

            point.Y = (float)destinationYAmount;

            point.X = (float)destinationXAmount;

            return point;
        }

        private byte[] GetPdfTemplate(int pageNumber)
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            string resourceTemplateName = string.Format("TrussSigner.PDF_Templates.Cover-{0}.pdf", pageNumber + 1);

            byte[] pdfContent;

            using (Stream stream = currentAssembly.GetManifestResourceStream(resourceTemplateName))
            {
                if (stream == null)
                {
                    return null;
                }

                pdfContent = new byte[stream.Length];

                stream.Read(pdfContent, 0, pdfContent.Length);
            }

            return pdfContent;
        }

        private void AppendPdfContent(PdfContentByte content, PdfWriter writer, int currentPage)
        {
            byte[] pdfContent = GetPdfTemplate(currentPage);

            PdfReader reader = new PdfReader(pdfContent);

            PdfImportedPage page = writer.GetImportedPage(reader, 1);

            content.AddTemplate(page, 0, 0);
        }

        private void AppendMetaDataFields(PdfContentByte cb)
        {
            foreach (TrussJobMetaDataField field in TrussJob.MetaData.Fields)
            {
                if (field.FieldPosition != null)
                {
                    BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                    cb.SetColorFill(BaseColor.BLACK);

                    cb.SetFontAndSize(bf, 10);

                    cb.BeginText();

                    cb.ShowTextAligned(0, field.FieldValue, field.FieldPosition.X, field.FieldPosition.Y, 0);

                    cb.EndText();
                }
            }
        }

        public void GenerateTrussSignedPdf()
        {
            if (TrussJob == null)
            {
                throw new NoNullAllowedException("Truss job is required");
            }

            if (TrussJob.TrussJobFilePath.IsNullOrWhiteSpace())
            {
                throw new NoNullAllowedException("TrussJobFilePath is required");
            }

            if (TrussJob.DestinationFilePath.IsNullOrWhiteSpace())
            {
                throw new NoNullAllowedException("Destination file path is required");
            }

            if (!File.Exists(TrussJob.TrussJobFilePath))
            {
                throw new FileNotFoundException("Truss file path is not a valid path", TrussJob.TrussJobFilePath);
            }

            List<string> trussFileContents = TrussJob.JobContents;

            int lineCounter = 0;

            int currentPage = 0;

            Rectangle documentSize = DetermineDocumentSize();

            using (Document document = new Document(documentSize))
            {
                FileStream fs = new FileStream(TrussJob.DestinationFilePath, FileMode.Create, FileAccess.Write);

                PdfWriter writer = PdfWriter.GetInstance(document, fs);

                document.Open();

                PdfContentByte cb = writer.DirectContent;

                foreach (string trussLine in trussFileContents)
                {
                    string[] splittrussLine = trussLine.Split("\\");

                    if (splittrussLine.SafeAny())
                    {
                        string[] splittrussName = splittrussLine[splittrussLine.Length - 1].Split(".");

                        string trussName = splittrussName[0];

                        BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                        cb.SetColorFill(BaseColor.BLACK);

                        cb.SetFontAndSize(bf, 10);

                        cb.BeginText();

                        TrussPoint point = DetermineLineLocation(lineCounter, currentPage);

                        // put the alignment and coordinates here
                        cb.ShowTextAligned(0, trussName, point.X, point.Y, 0);

                        cb.EndText();

                        lineCounter++;

                        if (lineCounter == GetMaxTrussByPage(currentPage))
                        {
                            lineCounter = 0;

                            AppendMetaDataFields(cb);

                            AppendPdfContent(cb, writer, currentPage);

                            currentPage ++;

                            document.NewPage();
                        }
                    }
                }

                AppendMetaDataFields(cb);

                AppendPdfContent(cb, writer, currentPage);

                document.Close();

                fs.Close();

                writer.Close();
            }
        }
    }
}

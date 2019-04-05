using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using UtilsLib.ExtensionMethods;

namespace BrightspaceXmlExamToPdf.Lib
{
    public class BrightspaceXmlExamPdfConverter
    {
        public BrightspaceXmlExamContext Context { get; set; }

        public BrightspaceXmlExamPdfConverter(BrightspaceXmlExamContext context)
        {
            Context = context;
        }

        public void CreatePdfFromContext(string fullNameOfPdf)
        {
            if (Context == null)
            {
                throw new InvalidOperationException("Context cannot be null");
            }

            WriteContextToPdf(fullNameOfPdf);
        }

        protected void WriteContextToPdf(string fullNameOfPdf)
        {
            FileStream fs = new FileStream(fullNameOfPdf, FileMode.Create);
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);

            document.Open();

            foreach (BrightspaceXmlExam exam in Context.Exams)
            {
                document.Add(new Paragraph(exam.ExamTitle));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));

                foreach (BrightspaceXmlExamQuestion question in exam.Questions)
                {
                    document.Add(new Paragraph(string.Format("Question {0}: {1} ({2} points)", question.QuestionIndex,
                        question.QuestionTitle, question.Weight.ToFixedDecimalPlace())));

                    foreach (BrightspaceXmlExamQuestionChoice choice in question.PossibleChoices)
                    {
                        if (choice.IsCorrectAnswer)
                        {
                            document.Add(new Paragraph(string.Format("{0}: {1}*", choice.ChoiceIndex, choice.ChoiceTitle)));
                        }
                        else
                        {
                            document.Add(new Paragraph(string.Format("{0}: {1}", choice.ChoiceIndex, choice.ChoiceTitle)));
                        }
                    }

                    document.Add(new Paragraph(" "));
                }
            }

            document.Close();
            writer.Close();
            fs.Close();
        }
    }
}

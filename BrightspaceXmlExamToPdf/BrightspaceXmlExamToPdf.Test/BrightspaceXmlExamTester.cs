using System;
using System.Xml;
using BrightspaceXmlExamToPdf.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrightspaceXmlExamToPdf.Test
{
    [TestClass]
    public class BrightspaceXmlExamTester
    {
        const string pathToXmlFile1 = @"TestXmls\questiondb_Group2.xml";
        const string pathToXmlFile2 = @"TestXmls\questiondb_group4.xml";
        const string pathToXmlFile3 = @"TestXmls\questiondb_Group6.xml";

        [TestMethod]
        public void XmlParseTest()
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(pathToXmlFile1);

            string json = JsonConvert.SerializeXmlNode(doc);

            dynamic d = JObject.Parse(json);

            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void MultipleChoiceTest()
        {
            BrightspaceXmlExamContext context = new BrightspaceXmlExamContext(pathToXmlFile1);

            Assert.IsNotNull(context.Exams);

            Assert.IsTrue(context.Exams.Count > 0);
        }

        [TestMethod]
        public void MultipleChoiceSingleSelectionTest()
        {
            BrightspaceXmlExamContext context = new BrightspaceXmlExamContext(pathToXmlFile2);

            Assert.IsNotNull(context.Exams);

            Assert.IsTrue(context.Exams.Count > 0);
        }

        [TestMethod]
        public void SingleExamTest()
        {
            BrightspaceXmlExamContext context = new BrightspaceXmlExamContext(pathToXmlFile3);

            Assert.IsNotNull(context.Exams);

            Assert.IsTrue(context.Exams.Count > 0);
        }

        [TestMethod]
        public void PdfConverterTest()
        {
            BrightspaceXmlExamContext context = new BrightspaceXmlExamContext(pathToXmlFile1);

            BrightspaceXmlExamPdfConverter pdfConverter = new BrightspaceXmlExamPdfConverter(context);

            pdfConverter.CreatePdfFromContext(@"TestXmls\questiondb_Group2.pdf");
        }
    }
}

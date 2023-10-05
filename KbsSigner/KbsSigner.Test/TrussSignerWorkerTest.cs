using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrussSigner.Test
{
    [TestClass]
    public class TrussSignerWorkerTest
    {
        [TestMethod]
        public void TrussSignerTest()
        {
            const string pathToKbs = @"C:\Source\Utils\KbsSigner\KbsSigner\bin\Debug\testpcl.PCL";

            const string pathToMeta = @"C:\Source\Utils\KbsSigner\KbsSigner\bin\Debug\testmeta.jobmeta";

            const string outputPath = @"C:\Source\Utils\KbsSigner\KbsSigner\bin\Debug\testpcl.pdf";

            TrussJob job = new TrussJob(pathToKbs, pathToMeta, outputPath);

            TrussSignerWorker worker = new TrussSignerWorker(job);  

            worker.GenerateTrussSignedPdf();
        }
    }
}

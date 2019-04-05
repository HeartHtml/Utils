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
            const string pathToKbs = @"C:\TrussSignerTest\SampleTrussJob\All trusses Building 1.pcl";

            TrussJob job = new TrussJob(pathToKbs, @"C:\TrussSignerTest\SampleTrussJob\CoverListTrusses.jobmeta", @"C:\TrussSignerTest\SampleTrussJob\TrussResult.pdf");

            TrussSignerWorker worker = new TrussSignerWorker(job);  

            worker.GenerateTrussSignedPdf();
        }
    }
}

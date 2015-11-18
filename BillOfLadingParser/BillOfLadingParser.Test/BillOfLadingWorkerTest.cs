using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BillOfLadingParser.Test
{
    [TestClass]
    public class BillOfLadingWorkerTest
    {
        [TestMethod]
        public void GenerateTest()
        {
            const string billOfLadingFilePath = @"C:\Users\horhe\Downloads\BillofLadingTest.pdf";

            BillOfLadingRequest request = new BillOfLadingRequest(billOfLadingFilePath);

            BillOfLadingWorker worker = new BillOfLadingWorker(request);

            worker.GenerateBillOfLadingMetaFile();;
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KbsSigner.Test
{
    [TestClass]
    public class KbsSignerWorkerTest
    {
        [TestMethod]
        public void KbsSignerTest()
        {
            const string pathToKbs = @"C:\Users\horhe\Downloads\Enviado a Georgie\wt-4521-A-L104.kbs";

            KbsSignerWorker worker = new KbsSignerWorker(pathToKbs);

            worker.GenerateKbsSignedPdf(@"C:\Users\horhe\Downloads\Enviado a Georgie\wt-4521-A-L104.pdf");
        }
    }
}

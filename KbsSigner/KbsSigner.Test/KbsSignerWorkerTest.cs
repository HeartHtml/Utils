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

            KbsJob job = new KbsJob(pathToKbs, @"C:\Users\horhe\Downloads\Enviado a Georgie\CoverListTrusses.jobmeta", @"C:\Users\horhe\Downloads\Enviado a Georgie\wt-4521-A-L104.pdf");

            KbsSignerWorker worker = new KbsSignerWorker(job);  

            worker.GenerateKbsSignedPdf();
        }
    }
}

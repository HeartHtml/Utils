using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void GenerateBillOfLadingMetaFile()
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
        }

        private void CreateBillOfLadingMetaFile(BillOfLadingResponse response)
        {
            File.Create(response.BillOfLadingMetaFilePath);
        }

        private BillOfLadingResponse ParseBillOfLadingFile()
        {
            BillOfLadingResponse response = new BillOfLadingResponse();

            FileInfo info = new FileInfo(Request.BillOfLadingFilePath);

            string metaFilePath = info.FullName.Replace(info.Extension, ".txt");

            response.BillOfLadingMetaFilePath = metaFilePath;



            return response;
        }
    }
}

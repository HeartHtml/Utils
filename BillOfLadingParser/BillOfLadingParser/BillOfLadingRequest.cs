using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillOfLadingParser
{
    public class BillOfLadingRequest
    {
        public string BillOfLadingFilePath { get; set; }

        public BillOfLadingRequest(string filePath)
        {
            BillOfLadingFilePath = filePath;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillOfLadingParser
{
    public class BillOfLadingResponse
    {
        public string BillOfLadingMetaFilePath { get; set; }

        public List<BillOfLadingMetaFileLine> MetaFileLines { get; set; }

        public BillOfLadingResponse()
        {
            MetaFileLines = new List<BillOfLadingMetaFileLine>();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (BillOfLadingMetaFileLine billOfLadingMetaFileLine in MetaFileLines)
            {
                builder.AppendLine(billOfLadingMetaFileLine.ToString());
            }

            return builder.ToString();
        }
    }
}

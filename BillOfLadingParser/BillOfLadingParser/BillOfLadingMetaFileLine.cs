using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsLib.ExtensionMethods;

namespace BillOfLadingParser
{
    public class BillOfLadingMetaFileLine
    {
        public string TrussId { get; set; }

        public List<string> AtList { get; set; }

        public BillOfLadingMetaFileLine()
        {
            AtList = new List<string>();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("ID:{0} ", TrussId);

            foreach (string atPiece in AtList)
            {
                builder.AppendFormat("{0} ", atPiece);
            }

            string export = builder.ToString();

            export = export.RemoveLastInstanceOfWord(" ");

            return export;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightspaceXmlExamToPdf.Lib
{
    public class BrightspaceXmlExamQuestionChoice
    {
        public string ChoiceTitle { get; set; }

        public string ChoiceId { get; set; }

        public int ChoiceIndex { get; set; }

        public bool IsCorrectAnswer { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", ChoiceTitle, ChoiceId, IsCorrectAnswer);
        }
    }
}

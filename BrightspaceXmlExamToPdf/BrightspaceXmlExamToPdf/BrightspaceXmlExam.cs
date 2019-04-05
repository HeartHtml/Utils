using System.Collections.Generic;

namespace BrightspaceXmlExamToPdf.Lib
{
    public class BrightspaceXmlExam
    {
        public List<BrightspaceXmlExamQuestion> Questions { get; set; }

        public string ExamTitle { get; set; }

        public BrightspaceXmlExam()
        {
            Questions = new List<BrightspaceXmlExamQuestion>();
        }

        public override string ToString()
        {
            return ExamTitle;
        }
    }
}

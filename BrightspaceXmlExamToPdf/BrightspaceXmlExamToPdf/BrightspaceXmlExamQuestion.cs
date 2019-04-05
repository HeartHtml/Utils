using System.Collections.Generic;

namespace BrightspaceXmlExamToPdf.Lib
{
    public class BrightspaceXmlExamQuestion
    {
        public string QuestionTitle { get; set; }

        public decimal Weight { get; set; }

        public int QuestionIndex { get; set; }

        public string QuestionType { get; set; }

        public List<BrightspaceXmlExamQuestionChoice> PossibleChoices { get; set; }

        public BrightspaceXmlExamQuestion()
        {
            PossibleChoices = new List<BrightspaceXmlExamQuestionChoice>();
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", QuestionTitle, Weight);
        }
    }
}

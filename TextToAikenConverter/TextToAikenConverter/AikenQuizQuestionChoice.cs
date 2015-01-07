using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextToAikenConverter
{
    public class AikenQuizQuestionChoice
    {
        public string QuestionIdentifier { get; set; }

        public string QuestionText { get; set; }

        public bool IsAnswer { get; set; }

        public override string ToString()
        {
            return string.Format("{0}. {1}", QuestionIdentifier, QuestionText);
        }
    }
}

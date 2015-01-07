using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextToAikenConverter
{
    public class AikenQuizQuestion
    {
        public string QuestionText { get; set; }

        public List<AikenQuizQuestionChoice> QuestionChoices { get; set; }

        public AikenQuizQuestionChoice Answer
        {
            get
            {
                return QuestionChoices.FirstOrDefault(dd => dd.IsAnswer);
            }
        }

        public AikenQuizQuestion()
        {
            QuestionChoices = new List<AikenQuizQuestionChoice>();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(QuestionText);

            foreach (AikenQuizQuestionChoice choice in QuestionChoices)
            {
                builder.AppendLine(choice.ToString());
            }

            if (Answer != null)
            {
                builder.AppendLine(string.Format("ANSWER: {0}", Answer.ChoiceIdentifier.ToUpper()));
            }

            return builder.ToString();
        }
    }
}

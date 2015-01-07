using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsLib.ExtensionMethods;

namespace TextToAikenConverter
{
    public enum EnumQuizType
    {
        None = 0,

        MultipleChoice = 1,

        TrueFalse = 2
    }

    public class AikenQuiz
    {
        public EnumQuizType QuizType { get; set; }

        public string Title { get; set; }

        public List<AikenQuizQuestion> AikenQuizQuestions { get; set; }

        public AikenQuiz(string[] sourceText)
        {
            AikenQuizQuestions = new List<AikenQuizQuestion>();

            Parse(sourceText);
        }

        private void Parse(string[] sourceText)
        {
            Title = sourceText[0];

            string quizType = sourceText[1];

            if (quizType.SafeEquals("Multiple Choice"))
            {
                QuizType = EnumQuizType.MultipleChoice;
            }
            else if (quizType.SafeEquals("True/False"))
            {
                QuizType = EnumQuizType.TrueFalse;
            }

            for (int i = 3; i < sourceText.Length; i ++)
            {
                
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Title);

            if (QuizType == EnumQuizType.MultipleChoice)
            {
                builder.AppendLine("Multiple Choice");
            }
            else
            {
                builder.AppendLine("True/False");
            }

            foreach (AikenQuizQuestion question in AikenQuizQuestions)
            {
                builder.AppendLine(question.ToString());
            }

            return builder.ToString();
        }
    }
}

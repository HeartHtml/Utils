using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private IEnumerable<string> ChoiceIdentifiers
        {
            get
            {
                return new[] {"a.", "b.", "c.", "d.", "e.", "f.", "g.", "h.", "i.", "j.", "k.", "l."};
            }
        }

        private string[] ChoiceAnswers
        {
            get
            {
                return new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l" };
            }
        }

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

            string rawQuizType = sourceText[1];

            if (rawQuizType.SafeEquals("Multiple Choice"))
            {
                QuizType = EnumQuizType.MultipleChoice;
            }
            else if (rawQuizType.SafeEquals("True/False"))
            {
                QuizType = EnumQuizType.TrueFalse;
            }

            bool questionCreated = false;

            bool choicesAddedForTrueFalse = false;

            AikenQuizQuestion question = null;

            string currentChoiceIdentifier = string.Empty;

            for (int i = 3; i < sourceText.Length; i ++)
            {
                if(sourceText[i].SafeEquals(Environment.NewLine) || sourceText[i].IsNullOrWhiteSpace() || sourceText[i].SafeEquals("\n\r"))
                {
                    AikenQuizQuestions.Add(question);

                    questionCreated = false;

                    choicesAddedForTrueFalse = false;

                    currentChoiceIdentifier = string.Empty;

                    continue;
                }

                if (!questionCreated)
                {
                    question = new AikenQuizQuestion
                    {
                        QuestionText = sourceText[i]
                    };

                    questionCreated = true;

                    continue;
                }

                if (QuizType == EnumQuizType.MultipleChoice)
                {
                    string choiceText = sourceText[i];

                    string choiceIdentifier = string.Empty;

                    foreach (string identifier in ChoiceIdentifiers)
                    {
                        if (choiceText.StartsWith(identifier))
                        {
                            choiceIdentifier = identifier;

                            currentChoiceIdentifier = choiceIdentifier;
                        }
                    }

                    if (choiceIdentifier.IsNullOrWhiteSpace())
                    {
                        AikenQuizQuestionChoice choice =
                            question.QuestionChoices.FirstOrDefault(
                                dd => dd.ChoiceIdentifier.SafeEquals(currentChoiceIdentifier));

                        if (choice != null)
                        {
                            choice.ChoiceText = choiceText.Append(choiceText);
                        }
                    }

                    else if (ChoiceAnswers.SafeAny(dd => dd.SafeEquals(choiceText)))
                    {
                        AikenQuizQuestionChoice choice =
                            question.QuestionChoices.FirstOrDefault(
                                dd => dd.ChoiceIdentifier.SafeEquals(choiceText));

                        if (choice != null)
                        {
                            choice.IsAnswer = true;
                        }
                    }
                    else
                    {
                        AikenQuizQuestionChoice questionChoice = new AikenQuizQuestionChoice
                        {
                            ChoiceIdentifier = choiceIdentifier.Replace(".", string.Empty),
                            ChoiceText = choiceText
                        };

                        question.QuestionChoices.Add(questionChoice);
                    }
                }
                else if (QuizType == EnumQuizType.TrueFalse)
                {
                    if (choicesAddedForTrueFalse)
                    {
                        string answerForTrueFalse = sourceText[i];

                        AikenQuizQuestionChoice choice =
                            question.QuestionChoices.FirstOrDefault(dd => dd.ChoiceText.SafeEquals(answerForTrueFalse));

                        if (choice != null)
                        {
                            choice.IsAnswer = true;
                        }
                    }
                    else
                    {
                        AikenQuizQuestionChoice choice1 = new AikenQuizQuestionChoice
                        {
                            ChoiceIdentifier = "A",
                            ChoiceText = "True"
                        };

                        AikenQuizQuestionChoice choice2 = new AikenQuizQuestionChoice
                        {
                            ChoiceIdentifier = "B",
                            ChoiceText = "False"
                        };

                        question.QuestionChoices.Add(choice1);

                        question.QuestionChoices.Add(choice2);

                        choicesAddedForTrueFalse = true;
                    }
                }
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextToAikenConverter
{

    public class AikenConverterHelper
    {
        public string SourceFilePath { get; set; }

        public AikenConverterHelper(string sourceFilePath)
        {
            if (File.Exists(sourceFilePath))
            {
                SourceFilePath = sourceFilePath;
            }
            else
            {
                throw new FileNotFoundException("Specified file does not exist.");
            }
        }

        public string GetQuizInRawAikenFormat()
        {
            string[] allLines = File.ReadAllLines(SourceFilePath);

            AikenQuiz quiz = new AikenQuiz(allLines);

            return quiz.ToString();
        }

    }
}

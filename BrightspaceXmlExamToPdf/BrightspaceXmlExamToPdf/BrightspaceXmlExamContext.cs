using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrightspaceXmlExamToPdf.Lib
{
    public class BrightspaceXmlExamContext
    {
        public List<BrightspaceXmlExam> Exams { get; set; }

        public string ContextName { get; set; }

        protected string RawJson { get; set; }

        public BrightspaceXmlExamContext(string pathToQuestionBank)
        {
            XmlDocument doc = new XmlDocument();

            FileInfo info = new FileInfo(pathToQuestionBank);

            ContextName = info.Name;

            doc.Load(pathToQuestionBank);

            RawJson = ConvertToJson(doc);

            Exams = ConvertFromJson(RawJson);
        }

        protected string ConvertToJson(XmlDocument doc)
        {
            string json = JsonConvert.SerializeXmlNode(doc);

            return json;
        }

        protected List<BrightspaceXmlExam> ConvertFromJson(string json)
        {
            dynamic d = JObject.Parse(json);

            List<BrightspaceXmlExam> exams = new List<BrightspaceXmlExam>();

            var sections = d.questestinterop.objectbank.section;

            if (sections is JArray)
            {
                foreach (var section in sections)
                {
                    BrightspaceXmlExam exam = ProcessSection(section);

                    exams.Add(exam);
                }
            }
            else
            {
                BrightspaceXmlExam exam = ProcessSection(sections);

                exams.Add(exam);
            }
            

            return exams;
        }

        protected BrightspaceXmlExam ProcessSection(dynamic section)
        {
            BrightspaceXmlExam exam = new BrightspaceXmlExam
            {
                ExamTitle = section["@title"]
            };

            int questionIndex = 1;

            foreach (var item in section.item)
            {
                BrightspaceXmlExamQuestion question = new BrightspaceXmlExamQuestion
                {
                    QuestionIndex = questionIndex
                };

                foreach (var qti_metadatafield in item.itemmetadata.qtimetadata.qti_metadatafield)
                {
                    if (qti_metadatafield.fieldlabel.Value.Equals("qmd_weighting"))
                    {
                        question.Weight = qti_metadatafield.fieldentry;
                    }
                }

                question.QuestionTitle = item.presentation.flow.material.mattext["#text"].Value;

                question.QuestionType = item["@title"];

                int choiceIndex = 1;

                foreach (var flow_label in item.presentation.flow.response_lid.render_choice.flow_label)
                {
                    BrightspaceXmlExamQuestionChoice questionChoice = new BrightspaceXmlExamQuestionChoice
                    {
                        ChoiceId = flow_label.response_label["@ident"],
                        ChoiceTitle = flow_label.response_label.flow_mat.material.mattext["#text"].Value,
                        ChoiceIndex = choiceIndex
                    };

                    question.PossibleChoices.Add(questionChoice);

                    choiceIndex++;
                }

                if (question.QuestionType.Equals("Multiple Choice"))
                {
                    foreach (var respcondition in item.resprocessing.respcondition)
                    {
                        string questionId = respcondition.conditionvar.varequal["#text"].Value;

                        decimal questionScore = Convert.ToDecimal(respcondition.setvar["#text"].Value);

                        if (questionScore > 0)
                        {
                            BrightspaceXmlExamQuestionChoice correctChoice =
                                question.PossibleChoices.First(dd => dd.ChoiceId.Equals(questionId));

                            correctChoice.IsCorrectAnswer = true;
                        }
                    }
                }
                else if (question.QuestionType.Equals("Multiple Correct Single Selection"))
                {
                    foreach (var respcondition in item.resprocessing.respcondition)
                    {
                        if (respcondition["@title"].Value.Equals("Scoring for the correct answers"))
                        {
                            foreach (var varequal in respcondition.conditionvar.varequal)
                            {
                                string questionId = varequal["#text"].Value;

                                BrightspaceXmlExamQuestionChoice correctChoice =
                                    question.PossibleChoices.First(dd => dd.ChoiceId.Equals(questionId));

                                correctChoice.IsCorrectAnswer = true;
                            }
                        }
                    }
                }
                else
                {
                    throw new NotSupportedException("Invalid question type received:" + question.QuestionType);
                }

                exam.Questions.Add(question);

                questionIndex++;
            }

            return exam;
        }
    }
}

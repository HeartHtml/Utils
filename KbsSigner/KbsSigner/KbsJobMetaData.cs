using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using UtilsLib.ExtensionMethods;

namespace KbsSigner
{

    public class KbsJobMetaData
    {
        public string MetaDataFilePath { get; set; }

        public List<KbsJobMetaDataField> Fields { get; set; }

        public string ParseErrorMessage { get; set; }

        public KbsJobMetaData(string metaDataFilePath)
        {
            MetaDataFilePath = metaDataFilePath;

            Fields = new List<KbsJobMetaDataField>();

            if (!MetaDataFilePath.IsNullOrWhiteSpace())
            {
                if (!File.Exists(MetaDataFilePath))
                {
                    throw new FileNotFoundException("Meta Data File not found");
                }

                ParseMetaDataFile();
            }
        }

        public KbsJobMetaData()
        {

        }

        private KbsPoint DetermineFieldPosition(EnumJobMetaField fieldType)
        {
            KbsPoint point = null;

            switch (fieldType)
            {
                case EnumJobMetaField.ProjectNameAndAddress:
                {
                    point = new KbsPoint
                    {
                        X = 160, 
                        Y = (float)675.5
                    };

                    break;
                }
                case EnumJobMetaField.Lot:
                {
                    point = new KbsPoint
                    {
                        X = 52, 
                        Y = 657
                    };

                    break;
                }
                case EnumJobMetaField.Block:
                {
                    point = new KbsPoint
                    {
                        X = 140,
                        Y = 657
                    };

                    break;
                }
                case EnumJobMetaField.County:
                {
                    point = new KbsPoint
                    {
                        X = 236,
                        Y = 657
                    };

                    break;
                }
                case EnumJobMetaField.ContractorBuilder:
                {
                    point = new KbsPoint
                    {
                        X = 126,
                        Y = 606
                    };

                    break;
                }
                case EnumJobMetaField.Model:
                {
                    point = new KbsPoint
                    {
                        X = 65,
                        Y = 591
                    };

                    break;
                }
                case EnumJobMetaField.Elevation:
                {
                    point = new KbsPoint
                    {
                        X = 254,
                        Y = 591
                    };

                    break;
                }
                case EnumJobMetaField.Options:
                {
                    point = new KbsPoint
                    {
                        X = 426,
                        Y = 591
                    };

                    break;
                }
                case EnumJobMetaField.OccupancySingleFamily:
                {
                    point = new KbsPoint
                    {
                        X = 495,
                        Y = 677
                    };

                    break;
                }
                case EnumJobMetaField.OccupancyMultiFamily:
                {
                    point = new KbsPoint
                    {
                        X = 495,
                        Y = 659
                    };

                    break;
                }
                case EnumJobMetaField.OccupancyCommercial:
                {
                    point = new KbsPoint
                    {
                        X = 495,
                        Y = 641
                    };

                    break;
                }
                case EnumJobMetaField.DesignCriteria:
                {
                    point = new KbsPoint
                    {
                        X = 147,
                        Y = 530
                    };

                    break;
                }
                case EnumJobMetaField.RfTopChordLiveLoad:
                {
                    point = new KbsPoint
                    {
                        X = 339,
                        Y = 538
                    };

                    break;
                }
                case EnumJobMetaField.RfTopChordDeadLoad:
                {
                    point = new KbsPoint
                    {
                        X = 339,
                        Y = 524 
                    };

                    break;
                }
                case EnumJobMetaField.RfBottomChordLiveLoad:
                {
                    point = new KbsPoint
                    {
                        X = 339,
                        Y = 510
                    };

                    break;
                }
                case EnumJobMetaField.RfBottomChordDeadLoad:
                {
                    point = new KbsPoint
                    {
                        X = 339,
                        Y = 497
                    };

                    break;
                }
                case EnumJobMetaField.RfDurationFactor:
                {
                    point = new KbsPoint
                    {
                        X = 339,
                        Y = 484
                    };

                    break;
                }
                case EnumJobMetaField.MeanHeight:
                {
                    point = new KbsPoint
                    {
                        X = 339,
                        Y = 470
                    };

                    break;
                }
                case EnumJobMetaField.Exposure:
                {
                    point = new KbsPoint
                    {
                        X = 339,
                        Y = 456
                    };

                    break;
                }
                case EnumJobMetaField.FlrTopChordLiveLoad:
                {
                    point = new KbsPoint
                    {
                        X = 540,
                        Y = 538
                    };

                    break;
                }
                case EnumJobMetaField.FlrTopChordDeadLoad:
                {
                    point = new KbsPoint
                    {
                        X = 540,
                        Y = 524
                    };

                    break;
                }
                case EnumJobMetaField.FlrBottomChordLiveLoad:
                {
                    point = new KbsPoint
                    {
                        X = 540,
                        Y = 510
                    };

                    break;
                }
                case EnumJobMetaField.FlrBottomChordDeadLoad:
                {
                    point = new KbsPoint
                    {
                        X = 540,
                        Y = 497
                    };

                    break;
                }
                case EnumJobMetaField.FlrDurationFactor:
                {
                    point = new KbsPoint
                    {
                        X = 540,
                        Y = 483
                    };

                    break;
                }
            }

            return point;
        }

        private void ParseMetaDataFile()
        {
            List<string> metaLinesList = File.ReadAllLines(MetaDataFilePath).ToList();

            StringBuilder builder = new StringBuilder();

            foreach (string line in metaLinesList)
            {
                KbsJobMetaDataField field = new KbsJobMetaDataField();

                string[] splitValues = line.Split("=", trimElements: true, options: StringSplitOptions.None);

                field.FieldType = EnumerationsHelper.ConvertFromString<EnumJobMetaField>(splitValues[0]);

                field.FieldValue = splitValues[1];

                if (!field.FieldValue.IsNullOrWhiteSpace() && field.FieldType != EnumJobMetaField.None)
                {
                    field.FieldPosition = DetermineFieldPosition(field.FieldType);

                    Fields.Add(field);
                }
                else
                {
                    if (field.FieldType == EnumJobMetaField.None)
                    {
                        builder.AppendLine(string.Format("Unable to parse line: {0}", line));
                    }
                }
            }

            ParseErrorMessage = builder.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using UtilsLib.ExtensionMethods;

namespace TrussSigner
{

    public class TrussJobMetaData
    {
        public string MetaDataFilePath { get; set; }

        public List<TrussJobMetaDataField> Fields { get; set; }

        public string ParseErrorMessage { get; set; }

        public TrussJobMetaData(string metaDataFilePath)
        {
            MetaDataFilePath = metaDataFilePath;

            Fields = new List<TrussJobMetaDataField>();

            if (!MetaDataFilePath.IsNullOrWhiteSpace())
            {
                if (!File.Exists(MetaDataFilePath))
                {
                    throw new FileNotFoundException("Meta Data File not found");
                }

                ParseMetaDataFile();
            }
        }

        public TrussJobMetaData()
        {

        }

        private TrussPoint DetermineFieldPosition(EnumJobMetaField fieldType)
        {
            TrussPoint point = null;

            switch (fieldType)
            {
                case EnumJobMetaField.ProjectNameAndAddress:
                {
                    point = new TrussPoint
                    {
                        X = 160, 
                        Y = (float)675.5
                    };

                    break;
                }
                case EnumJobMetaField.Lot:
                {
                    point = new TrussPoint
                    {
                        X = 52, 
                        Y = 657
                    };

                    break;
                }
                case EnumJobMetaField.Block:
                {
                    point = new TrussPoint
                    {
                        X = 140,
                        Y = 657
                    };

                    break;
                }
                case EnumJobMetaField.County:
                {
                    point = new TrussPoint
                    {
                        X = 236,
                        Y = 657
                    };

                    break;
                }
                case EnumJobMetaField.ContractorBuilder:
                {
                    point = new TrussPoint
                    {
                        X = 126,
                        Y = 606
                    };

                    break;
                }
                case EnumJobMetaField.Model:
                {
                    point = new TrussPoint
                    {
                        X = 65,
                        Y = 591
                    };

                    break;
                }
                case EnumJobMetaField.Elevation:
                {
                    point = new TrussPoint
                    {
                        X = 254,
                        Y = 591
                    };

                    break;
                }
                case EnumJobMetaField.Options:
                {
                    point = new TrussPoint
                    {
                        X = 426,
                        Y = 591
                    };

                    break;
                }
                case EnumJobMetaField.OccupancySingleFamily:
                {
                    point = new TrussPoint
                    {
                        X = 495,
                        Y = 677
                    };

                    break;
                }
                case EnumJobMetaField.OccupancyMultiFamily:
                {
                    point = new TrussPoint
                    {
                        X = 495,
                        Y = 659
                    };

                    break;
                }
                case EnumJobMetaField.OccupancyCommercial:
                {
                    point = new TrussPoint
                    {
                        X = 495,
                        Y = 641
                    };

                    break;
                }
                case EnumJobMetaField.DesignCriteria:
                {
                    point = new TrussPoint
                    {
                        X = 147,
                        Y = 530
                    };

                    break;
                }
                case EnumJobMetaField.RfTopChordLiveLoad:
                {
                    point = new TrussPoint
                    {
                        X = 339,
                        Y = 538
                    };

                    break;
                }
                case EnumJobMetaField.RfTopChordDeadLoad:
                {
                    point = new TrussPoint
                    {
                        X = 339,
                        Y = 524 
                    };

                    break;
                }
                case EnumJobMetaField.RfBottomChordLiveLoad:
                {
                    point = new TrussPoint
                    {
                        X = 339,
                        Y = 510
                    };

                    break;
                }
                case EnumJobMetaField.RfBottomChordDeadLoad:
                {
                    point = new TrussPoint
                    {
                        X = 339,
                        Y = 497
                    };

                    break;
                }
                case EnumJobMetaField.RfDurationFactor:
                {
                    point = new TrussPoint
                    {
                        X = 339,
                        Y = 484
                    };

                    break;
                }
                case EnumJobMetaField.MeanHeight:
                {
                    point = new TrussPoint
                    {
                        X = 339,
                        Y = 470
                    };

                    break;
                }
                case EnumJobMetaField.Exposure:
                {
                    point = new TrussPoint
                    {
                        X = 339,
                        Y = 456
                    };

                    break;
                }
                case EnumJobMetaField.FlrTopChordLiveLoad:
                {
                    point = new TrussPoint
                    {
                        X = 540,
                        Y = 538
                    };

                    break;
                }
                case EnumJobMetaField.FlrTopChordDeadLoad:
                {
                    point = new TrussPoint
                    {
                        X = 540,
                        Y = 524
                    };

                    break;
                }
                case EnumJobMetaField.FlrBottomChordLiveLoad:
                {
                    point = new TrussPoint
                    {
                        X = 540,
                        Y = 510
                    };

                    break;
                }
                case EnumJobMetaField.FlrBottomChordDeadLoad:
                {
                    point = new TrussPoint
                    {
                        X = 540,
                        Y = 497
                    };

                    break;
                }
                case EnumJobMetaField.FlrDurationFactor:
                {
                    point = new TrussPoint
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
                TrussJobMetaDataField field = new TrussJobMetaDataField();

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

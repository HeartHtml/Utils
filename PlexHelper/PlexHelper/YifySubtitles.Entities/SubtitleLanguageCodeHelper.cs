using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsLib.ExtensionMethods;

namespace PlexHelper.YifySubtitles.Entities
{
    public class SubtitleLanguageCodeHelper
    {
        public static string[] LanguageCodes
        {
            get
            {
                return new[] {".en", ".spa", ".fr"};
            }
        }

        public static string ConvertToLanguageCode(string fullyQualifiedLanguage)
        {
            switch (fullyQualifiedLanguage)
            {
                case "English":
                    {
                        return ".en";
                    }
                case "Spanish":
                    {
                        return ".spa";
                    }
                case "French":
                    {
                        return ".fr";
                    }
                default:
                    {
                        throw new InvalidOperationException("Received unsupported language");
                    }
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace UtilsLib.ExtensionMethods
{

    /// <summary>
    /// Extension Methods for Booleans
    /// </summary>
    public static class BooleanExtensions 
    {

        /// <summary>
        /// The internal string representation of yes.
        /// </summary>
        public static string YesString = "Yes";

        /// <summary>
        /// The internal string representation of no.
        /// </summary>
        public static string NoString = "No";

        /// <summary>
        /// The internal string representation of yes shortened.
        /// </summary>
        public static string YesStringShort = "Y";

        /// <summary>
        /// The internal string representation of no shortened.
        /// </summary>
        public static string NoStringShort = "N";

        /// <summary>
        /// 
        /// </summary>
        public static string[] BooleanConstants
        {
            get
            {
                List<string> boolConstants = new List<string>();

                boolConstants.AddRange(TrueConstants);

                boolConstants.AddRange(FalseConstants);

                return boolConstants.ToArray();
            }
        }

        /// <summary>
        /// True boolean english constants
        /// </summary>
        public static string[] TrueConstants
        {
            get
            {
                return new[]
                    {
                        bool.TrueString,
                        YesString,
                        YesStringShort
                    };

            }
        }

        /// <summary>
        /// False boolean english constants
        /// </summary>
        public static string[] FalseConstants
        {
            get
            {
                return new[]
                    {
                        bool.FalseString,
                        NoString,
                        NoStringShort
                    };

            }
        }

        /// <summary>
        /// Returns a friendly string representation of the boolean value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="trueString">Custom value if desired</param>
        /// <param name="falseString">Custom value if desired</param>
        /// <returns>A string</returns>
        public static string ToFormattedString(this bool value, string trueString = "Yes", string falseString = "No")
        {
            return value ? trueString : falseString;
        }

        /// <summary>
        /// Attempts to parse a boolean value using english constants
        /// </summary>
        /// <param name="valueToParse"></param>
        /// <param name="parsedValue"></param>
        /// <returns>True if parsed, false otherwise</returns>
        public static bool TryParseBooleanConstants(string valueToParse, out bool parsedValue)
        {
            parsedValue = false;

            if (valueToParse.IsNullOrWhiteSpace())
            {
                return false;
            }

            string trimmedValue = valueToParse.TrimSafely();

            if (BooleanConstants.Contains(trimmedValue, StringComparison.CurrentCultureIgnoreCase))
            {
                if (TrueConstants.Contains(trimmedValue, StringComparison.CurrentCultureIgnoreCase))
                {
                    parsedValue = true;
                }
                else if (FalseConstants.Contains(trimmedValue, StringComparison.CurrentCultureIgnoreCase))
                {
                    parsedValue = false;
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to parse a boolean string using boolean strings
        /// </summary>
        /// <param name="valueToParse"></param>
        /// <param name="parsedValue"></param>
        /// <returns>True if parsed, false otherwise</returns>
        public static bool TryParse(string valueToParse, out bool parsedValue)
        {
            return bool.TryParse(valueToParse, out parsedValue);
        }

    }
}

using System;

namespace UtilsLib.ExtensionMethods
{
    /// <summary>
    /// Extension Methods for Integers
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        /// Converts Numeric to Text
        /// </summary>
        /// <param name="number" />
        /// <returns>Text result of the given number</returns>
        public static string NumberToWords(this int number)
        {
            if (number == 0)
            {
                return "zero";
            }

            if (number < 0)
            {
                return string.Format("minus {0}", NumberToWords(Math.Abs(number)));
            }

            string words = string.Empty;

            if ((number / 1000000) > 0)
            {
                words += string.Format("{0} million ", NumberToWords(number / 1000000));
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += string.Format("{0} thousand ", NumberToWords(number / 1000));
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += string.Format("{0} hundred ", NumberToWords(number / 100));
                number %= 100;
            }

            if (number > 0)
            {
                if (!string.IsNullOrEmpty(words))
                {
                    words += "and ";
                }

                var unitsMap
                    = new[]
                          {
                              "zero", 
                              "one", 
                              "two", 
                              "three", 
                              "four", 
                              "five", 
                              "six", 
                              "seven", 
                              "eight", 
                              "nine", 
                              "ten", 
                              "eleven", 
                              "twelve", 
                              "thirteen", 
                              "fourteen", 
                              "fifteen", 
                              "sixteen", 
                              "seventeen", 
                              "eighteen", 
                              "nineteen"
                          };

                var tensMap
                    = new[]
                          {
                              "zero", 
                              "ten", 
                              "twenty", 
                              "thirty", 
                              "forty", 
                              "fifty", 
                              "sixty", 
                              "seventy", 
                              "eighty", 
                              "ninety"
                          };

                if (number < 20)
                {
                    words += unitsMap[number];
                }
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                    {
                        words += string.Format("-{0}", unitsMap[number % 10]);
                    }
                }
            }

            if (!string.IsNullOrEmpty(words))
            {
                words = words.ToTitleCase();
            }

            return words;
        }

        /// <summary>
        /// Returns the absolute value of int
        /// </summary>
        /// <param name="value" />
        /// <returns>The absolute value of the int</returns>
        public static int AbsoluteValue(this int value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the negative value of int
        /// </summary>
        /// <param name="value" />
        /// <returns>The negative value of the int</returns>
        public static int NegativeValue(this int value)
        {
            return value.AbsoluteValue() * -1;
        }

        /// <summary>
        /// Returns whether or not int is between upper and lower bounds
        /// </summary>
        /// <param name="value" />
        /// <param name="lowerBound">The lower bound to compare</param>
        /// <param name="upperBound">The upper bound to compare</param>
        /// <param name="includeBounds">[OPTIONAL] Whether or not to include the bounds</param>
        /// <returns>Returns true, if value is between the bounds</returns>
        public static bool Between(this int value, int lowerBound, int upperBound, bool includeBounds = true)
        {
            return includeBounds
                       ? value >= lowerBound && value <= upperBound
                       : value > lowerBound && value < upperBound;

        }

        /// <summary>
        /// Returns whether or not int is negative
        /// </summary>
        /// <param name="value" />
        /// <param name="includeZero"/>
        /// <returns>Returns true, if value is negative</returns>
        public static bool IsNegative(this int value, bool includeZero = false)
        {
            return includeZero ? value <= 0 : value < 0;
        }

        /// <summary>
        /// Returns whether or not int is positive
        /// </summary>
        /// <param name="value" />
        /// <param name="includeZero"/>
        /// <returns>Returns true, if value is positive</returns>
        public static bool IsPositive(this int value, bool includeZero = false)
        {
            return includeZero ? value >= 0 : value > 0;
        }

        /// <summary>
        /// Adds a leading zero to number.
        /// </summary>
        /// <param name="value">Number to add leading zero to.</param>
        /// <returns>String containing a number with a zero if less than 10.</returns>
        public static string AddLeadingZero(this int value)
        {
            return value < 10 && value > -1 ? string.Format("0{0}", value) : value.ToString();
        }
    }
}

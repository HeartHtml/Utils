using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace UtilsLib.ExtensionMethods
{
	/// <summary>
	/// Extension Methods for String
	/// </summary>
	public static class StringExtensions
	{
	    /// <summary>
        /// New line character: \n
        /// </summary>
        public static string NewLineStringLiteral
        {
            get
            {
                return "\n";
            }
        }

		/// <summary>
		/// New line in HTML environment
		/// </summary>
		public static string BreakTag
		{
			get
			{
			    return "<br />";
			}
		}

		/// <summary>
		/// Environment new line that works with HTML
		/// </summary>
		/// <param name="isHtml"></param>
		/// <returns></returns>
		public static string EnvironmentNewLine(bool isHtml)
		{
			return isHtml ? BreakTag : Environment.NewLine;
		}

        /// <summary>
        /// Generic method to generate property changed notes
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="oldPropertyValue"></param>
        /// <param name="newPropertyValue"></param>
        /// <returns></returns>
        public static string GeneratePropertyChangedString(string propertyName, string oldPropertyValue, string newPropertyValue)
        {
            return string.Format("<b>{0}</b>[ {1} --> {2} ]", propertyName, oldPropertyValue, newPropertyValue);
        }

		#region String Methods

        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int ComputeLevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        /// <summary>
	    /// Replaces all occurrences of white space in this string to the specified wildcard character
	    /// </summary>
	    /// <param name="value"></param>
	    /// <param name="wildcard"></param>
	    /// <returns></returns>
	    public static string ToWildcard(this string value, char wildcard = '%')
        {
            if (value.IsNullOrWhiteSpace())
            {
                return null;
            }

            StringBuilder builder = new StringBuilder();

            foreach (char characterAtIndex in value)
            {
                builder.Append(char.IsWhiteSpace(characterAtIndex) ? wildcard : characterAtIndex);
            }

	        return builder.ToString();
        }

        /// <summary>
        /// Get substring of specified number of characters on the left.
        /// </summary>
        public static string Left(this string value, int length)
        {
            return value.Substring(0, Math.Min(value.Length, length));
        }

        /// <summary>
        /// Get substring of specified number of characters on the right.
        /// </summary>
        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }

		/// <summary>
		/// Removes last instance of comma
		/// </summary>
		/// <param name="value" />
		/// <returns />
		public static string RemoveLastInstanceOfComma(this string value)
		{
			return value.RemoveLastInstanceOfWord(",").TrimSafely();
		}

		/// <summary>
		/// Removes last instance of semicolon
		/// </summary>
		/// <param name="value" />
		/// <returns />
		public static string RemoveLastInstanceOfSemicolon(this string value)
		{
			return value.RemoveLastInstanceOfWord(";").TrimSafely();
		}

		/// <summary>
		/// Removes last instance of character
		/// </summary>
		/// <param name="value" />
		/// <param name="wordToRemove" />
		/// <returns />
		public static string RemoveLastInstanceOfWord(this string value, string wordToRemove)
		{
			if (!string.IsNullOrEmpty(value) && value.Length > 0 && value.ToLower().Contains(wordToRemove.ToLower()))
			{
				int lastIndexOfWord = value.LastIndexOf(wordToRemove, StringComparison.CurrentCultureIgnoreCase);

				if (lastIndexOfWord >= 0)
				{
					value = value.Remove(lastIndexOfWord, wordToRemove.Length);
				}
			}

			return value.TrimSafely();
		}

		/// <summary>
		/// Removes last instance of delimiter
		/// </summary>
		/// <param name="value" />
		/// <param name="delimiter" />
		/// <returns />
		public static string RemoveLastInstanceOfWord(this string value, params char[] delimiter)
		{
			value = delimiter.Aggregate(value, (current, c) => RemoveLastInstanceOfWord(current, c.ToString(CultureInfo.InvariantCulture)));
			return value.TrimSafely();
		}

	    /// <summary>
	    /// Removes all numeric characters from a string
	    /// </summary>
	    /// <param name="value"></param>
	    /// <returns></returns>
	    public static string RemoveNumerics(this string value)
	    {
	        List<char> charactersInString = value.ToCharArray().ToList();

	        string newString = value;

	        foreach (char c in charactersInString)
	        {
	            if (IsNumeric(c.ToString(CultureInfo.InvariantCulture)))
	            {
                    newString = newString.Replace(c.ToString(CultureInfo.InvariantCulture), string.Empty);
	            }
	        }

	        return newString;
	    }

	    /// <summary>
	    /// Removes all alphabetical characters from a string
	    /// </summary>
	    /// <param name="value"></param>
	    /// <returns></returns>
	    public static string RemoveAlphas(this string value)
	    {
            List<char> charactersInString = value.ToCharArray().ToList();

            string newString = value;

            foreach (char c in charactersInString)
            {
                if (IsAlpha(c.ToString(CultureInfo.InvariantCulture)))
                {
                    newString = newString.Replace(c.ToString(CultureInfo.InvariantCulture), string.Empty);
                }
            }

            return newString;
	    }

		/// <summary>
		/// Converts a sequence of city, state zip, into the mailing friendly format similar to Miami, FL 33166
		/// </summary>
		/// <param name="city" />
		/// <param name="stateCode" />
		/// <param name="zip" />
		/// <returns />
		public static string ConvertToAddressFriendly(string city, string stateCode, string zip)
		{
			return string.Format("{0}, {1} {2}", city.TrimSafely(), stateCode.TrimSafely(), zip.TrimSafely());
		}

		/// <summary>
		/// Trims a string without the risk of null exception
		/// </summary>
		/// <param name="value">String to trim</param>
		/// <param name="delims">Delimiters to trim</param>
		/// <returns>Safely trimmed string</returns>
		public static string TrimSafely(this string value, params char[] delims)
		{
			if (delims == null)
			{
				delims = new[] {' '};
			}

			return string.IsNullOrEmpty(value) ? value : value.Trim(delims);
		}

		/// <summary>
		/// Trims a string if it is longer than the given length.
		/// </summary>
		/// <param name="value">String to trim.</param>
		/// <param name="length">Max length string should be.</param>
		/// <returns>String containing less or equal to the given length.</returns>
		public static string TrimMax(this string value, int length)
		{
			return value.TrimSafely().SafeSubstring(length);
		}

		/// <summary>
		/// Checks if a string is null or empty
		/// </summary>
		/// <param name="value" />
		/// <returns>True if null or empty, false otherwise</returns>
		public static bool IsNullOrEmpty(this string value)
		{
			return string.IsNullOrEmpty(value);
		}

		/// <summary>
		/// Checks if a string is null or white space
		/// </summary>
		/// <param name="value" />
		/// <returns>True if null or white space, or empty</returns>
		public static bool IsNullOrWhiteSpace(this string value)
		{
			return value.TrimSafely().IsNullOrEmpty();
		}

		/// <summary>
		/// Removes all chars from string
		/// </summary>
		/// <param name="value" />
		/// <param name="delims" />
		/// <returns />
		public static string RemoveChars(this string value, params char[] delims)
		{
			return value.IsNullOrWhiteSpace()
				       ? value
				       : value.RemoveStrings(delims.Select(delim => delim.ToString(CultureInfo.InvariantCulture)).ToArray());
		}

		/// <summary>
		/// Removes all strings from string
		/// </summary>
		/// <param name="value" />
		/// <param name="delims" />
		/// <returns />
		public static string RemoveStrings(this string value, params string[] delims)
		{
			if (!value.IsNullOrWhiteSpace())
			{
				value = delims.Aggregate(value, (current, delim) => current.Replace(delim, string.Empty));
			}

			return value;
		}

        /// <summary>
        /// Returns the phone number split into 3 individual chunks, i.e xxx at pos 0, xxx at pos 1, xxxx at pos 2,
        /// the rest is discarded
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        public static string[] SplitPhoneNumber(this string phoneNum)
        {
            List<string> splitPhoneNumber = new List<string>();

            string[] splitArray = new string[0];

            if (!phoneNum.IsNullOrWhiteSpace())
            {
                phoneNum = phoneNum.RemoveStrings("(", ")", "-", " ").TrimSafely();

                string piece = string.Empty;

                try
                {
                    for (int i = 0; i < 3; i++)
                    {
                        piece = piece.Append(phoneNum[i].ToString(CultureInfo.InvariantCulture));
                    }

                    splitPhoneNumber.Add(piece);

                    piece = string.Empty;

                    for (int i = 3; i < 6; i++)
                    {
                        piece = piece.Append(phoneNum[i].ToString(CultureInfo.InvariantCulture));
                    }

                    splitPhoneNumber.Add(piece);

                    piece = string.Empty;

                    for (int i = 6; i < 10; i++)
                    {
                        piece = piece.Append(phoneNum[i].ToString(CultureInfo.InvariantCulture));
                    }

                    splitPhoneNumber.Add(piece);

                    splitArray = splitPhoneNumber.ToArray();
                }
                catch (Exception)
                {
                    splitArray = new string[0];
                }
            }

            return splitArray;
        }


	    /// <summary>
	    /// Tries to format the phone to the US standard, if unable then it returns the original value
	    /// </summary>
	    /// <param name="phoneNum" />
	    /// <param name="includeAreaCodeParenthesis"></param>
	    /// <returns />
	    public static string FormatPhoneNumber(this string phoneNum, bool includeAreaCodeParenthesis = false)
		{
			string origPhone = phoneNum;
			string valueToreturn = phoneNum;

			try
			{
				if (!phoneNum.IsNullOrWhiteSpace())
				{
					phoneNum = phoneNum.RemoveStrings("(", ")", "-", " ").TrimSafely();

					if (phoneNum.IsNumeric())
					{
						switch (phoneNum.Length)
						{
							case 10:
						        if (includeAreaCodeParenthesis)
						        {
						            valueToreturn = string.Format("({0})-{1}-{2}",
						                                          phoneNum.Substring(0, 3),
						                                          phoneNum.Substring(3, 3),
						                                          phoneNum.Substring(6, 4));
						        }
						        else
						        {
                                    valueToreturn = string.Format("{0}-{1}-{2}",
                                                                  phoneNum.Substring(0, 3),
                                                                  phoneNum.Substring(3, 3),
                                                                  phoneNum.Substring(6, 4));
						        }
						        break;

							case 7:
								valueToreturn = string.Format("{0}-{1}",
								                              phoneNum.Substring(0, 3),
								                              phoneNum.Substring(4, 4));
								break;
						}
					}
				}
			}
			catch (Exception)
			{
				valueToreturn = origPhone;
			}

			return valueToreturn;
		}

	    /// <summary>
	    /// Formats Address
	    /// </summary>
	    /// <param name="addressLine1" />
	    /// <param name="addressLine2" />
	    /// <param name="city" />
	    /// <param name="state" />
	    /// <param name="zipCode" />
	    /// <param name="displayOnOneLine"></param>
	    /// <param name="useHtml"></param>
	    /// <returns>Formatted Address</returns>
	    public static string FormatAddress(string addressLine1,
		                                   string addressLine2,
		                                   string city,
		                                   string state,
		                                   string zipCode,
		                                   bool displayOnOneLine = false, 
		                                   bool useHtml = true)
		{
			StringBuilder firstHalf = new StringBuilder(),
			              secHalf = new StringBuilder();

			string compAdd;
			char[] delims = {',', ' '};

			if (!addressLine1.IsNullOrWhiteSpace())
			{
				firstHalf.Append(addressLine1);
			}

			if (!addressLine2.IsNullOrWhiteSpace())
			{
				firstHalf.AppendFormat(" {0}", addressLine2);
			}

			if (!city.IsNullOrWhiteSpace())
			{
				secHalf.Append(city);
			}

			if (!state.IsNullOrWhiteSpace())
			{
				secHalf.AppendFormat(", {0}", state);
			}

			if (!zipCode.IsNullOrWhiteSpace())
			{
				secHalf.AppendFormat(" {0}", zipCode);
			}

			string firstHalfClean = firstHalf.ToString().Trim(delims);
			string secHalfClean = secHalf.ToString().Trim(delims);

			if (firstHalfClean.IsNullOrWhiteSpace() && secHalfClean.IsNullOrWhiteSpace())
			{
				compAdd = "Address N/A";
			}
			else
			{
				if (firstHalfClean.IsNullOrWhiteSpace())
				{
					compAdd = secHalfClean;
				}
				else if (secHalfClean.IsNullOrWhiteSpace())
				{
					compAdd = firstHalfClean;
				}
				else
				{
					compAdd = string.Format("{0}{1}{2}", firstHalfClean, GetLineDelimiter(displayOnOneLine, useHtml), secHalfClean);
				}
			}

			return compAdd;
		}

		private static string GetLineDelimiter(bool displayOnOneLine, bool useHtml)
		{
			string delimiter = " ";
			if (!displayOnOneLine)
			{
				delimiter = EnvironmentNewLine(useHtml);
			}
			return delimiter;
		}

		/// <summary>
		/// Overload for Contains which can take a StrongComparison parameter
		/// </summary>
		/// <param name="value" />
		/// <param name="valueToCheck" />
		/// <param name="comparison" />
		/// <returns />
		public static bool Contains(this string value, string valueToCheck, StringComparison comparison)
		{
			bool contains = false;

			if (!value.IsNullOrWhiteSpace())
			{
				switch (comparison)
				{
					case StringComparison.CurrentCultureIgnoreCase:
					case StringComparison.InvariantCultureIgnoreCase:
					case StringComparison.OrdinalIgnoreCase:
						contains = value.ToUpper().TrimSafely().Contains(valueToCheck.ToUpper().TrimSafely());
						break;

					default:
						contains = value.TrimSafely().Contains(valueToCheck.TrimSafely());
						break;
				}
			}

			return contains;
		}

		/// <summary>
		/// Returns true if value contains any value in valueToCheck
		/// </summary>
		/// <param name="value" />
		/// <param name="valuesToCheck" />
		/// <param name="comparison" />
		/// <returns />
		public static bool ContainsAny(this string value,
		                               IEnumerable<string> valuesToCheck,
		                               StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
		{
			bool contains = false;

			if (!value.IsNullOrWhiteSpace())
			{
				foreach (string valueToCheck in valuesToCheck)
				{
					switch (comparison)
					{
						case StringComparison.CurrentCultureIgnoreCase:
						case StringComparison.InvariantCultureIgnoreCase:
						case StringComparison.OrdinalIgnoreCase:
							contains = value.ToUpper().TrimSafely().Contains(valueToCheck.ToUpper().TrimSafely());
							break;

						default:
							contains = value.TrimSafely().Contains(valueToCheck.TrimSafely());
							break;
					}

					if (contains)
					{
						break;
					}
				}
			}

			return contains;
		}

		/// <summary>
		/// Appends a string onto another
		/// </summary>
		/// <param name="value" />
		/// <param name="stringToAppend" />
		/// <returns />
		public static string Append(this string value, string stringToAppend)
		{
			string returnString = value;

			if (returnString.IsNullOrWhiteSpace())
			{
				returnString = string.Empty;
			}

			returnString += stringToAppend;

			return returnString;
		}

		/// <summary>
		/// Appends object params onto a string, using string.Format()
		/// The string this method is extending must have the param instances ({0}, {1}, etc)
		/// </summary>
		/// <param name="value" />
		/// <param name="objectToAppend" />
		/// <returns />
		public static string AppendFormat(this string value, params object[] objectToAppend)
		{
			string returnString = value;

			if (!returnString.IsNullOrWhiteSpace() && objectToAppend != null)
			{
				int numberOfParams = objectToAppend.Length;
				int numberOfLeftBrace = returnString.Count(s => s.Equals('{'));
				int numberOfRightBrace = returnString.Count(s => s.Equals('}'));

				if (numberOfParams == numberOfLeftBrace && numberOfParams == numberOfRightBrace)
				{
					bool allParamsFound = true;

					for (int i = 0; i < numberOfParams; i++)
					{
						if (!returnString.Contains(string.Format("{{{0}}}", i)))
						{
							allParamsFound = false;
							break;
						}
					}

					if (allParamsFound)
					{
						returnString = string.Format(returnString, objectToAppend);
					}
				}
			}

			return returnString;
		}

		/// <summary>
		/// Removes any extra spaces from a string (Double-space, triple-space, etc.)
		/// </summary>
		/// <param name="value" />
		/// <returns>A string without extra spaces</returns>
		public static string RemoveExtraSpaces(this string value)
		{
			string retValue = value.TrimSafely();

			if (!retValue.IsNullOrWhiteSpace())
			{
				while (retValue.Contains(@"  "))
				{
					retValue = retValue.Replace(@"  ", @" ");
					retValue = retValue.TrimSafely();
				}
			}

			return retValue;
		}

		/// <summary>
		/// Converts first letter of every word in the string to uppercase
		/// </summary>
		/// <param name="value" />
		/// <returns>Converts first letter of every word in the string to uppercase</returns>
		public static string ToTitleCase(this string value)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
		}

		/// <summary>
		/// Reverses a string
		/// </summary>
		/// <param name="value" />
		/// <returns>The reversed string</returns>
		public static string Reverse(this string value)
		{
			string retValue = value.TrimSafely();

		    string temp = string.Empty;

			if (!retValue.IsNullOrWhiteSpace())
			{
			    for(int i = retValue.Length - 1; i >= 0; i --)
			    {
			        temp = temp.Append(retValue[i].ToString(CultureInfo.InvariantCulture));
			    }
			}

            return temp;
		}

		/// <summary>
		/// Surrounds text with double quotes
		/// </summary>
		/// <param name="value" />
		/// <returns />
		public static string DoubleQuotify(this string value)
		{
			return SurroundWith(value, "\"");
		}

		/// <summary>
		/// Surrounds text with string passed in
		/// </summary>
		/// <param name="value" />
		/// <param name="ends" />
		/// <returns />
		public static string SurroundWith(this string value, string ends)
		{
			return string.Format("{0}{1}{0}", ends, value);
		}

		/// <summary>
		/// Converts string to uppercase safely
		/// </summary>
		/// <param name="value" />
		/// <returns />
		public static string ToUpperSafely(this string value)
		{
			return value.IsNullOrWhiteSpace() ? value : value.ToUpper();
		}

		/// <summary>
		/// Converts string to lowercase safely
		/// </summary>
		/// <param name="value" />
		/// <returns />
		public static string ToLowerSafely(this string value)
		{
			return value.IsNullOrWhiteSpace() ? value : value.ToLower();
		}

		/// <summary>
		/// Safely gets Substring of word
		/// </summary>
		/// <param name="value" />
		/// <param name="length">Length of substring</param>
		/// <param name="startIndex">[OPTIONAL] Start index</param>
		/// <returns>Substring of original string</returns>
		public static string SafeSubstring(this string value, int length, int startIndex = 0)
		{
			return value.IsNullOrWhiteSpace()
				       ? value
				       : value.Length > length ? value.Substring(startIndex, length) : value;
		}

        /// <summary>
        /// Removes all characters not allowed in file names
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
	    public static string RemoveInvalidFileNameChars(this string value)
	    {
            return value.IsNullOrWhiteSpace() ? value : value.RemoveChars(Path.GetInvalidFileNameChars());
	    }

		/// <summary>
		/// Removes all characters not considered desirable in a string
		/// </summary>
		/// <param name="value" />
		/// <returns>The supplied string, minus invalid characters</returns>
		public static string RemoveInvalidCharacters(this string value)
		{
			return value.IsNullOrWhiteSpace() ? value : value.RemoveChars(Path.GetInvalidFileNameChars()).RemoveChars(new[] {'%', '&', ',', '.', '(', ')'});
		}

		/// <summary>
		/// Safely Determines whether one string equals another 
		/// (If both are null/empty, then they're equal)
		/// </summary>
		/// <param name="value" />
		/// <param name="valueToCompare" />
		/// <param name="comparison" />
		/// <returns>True, if strings are equal</returns>
		public static bool SafeEquals(this string value,
		                              string valueToCompare,
		                              StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
		{
			return value.IsNullOrWhiteSpace() && valueToCompare.IsNullOrWhiteSpace() ||
			       !value.IsNullOrWhiteSpace() && value.Equals(valueToCompare, comparison);
		}

		/// <summary>
		/// Returns a list of all chars (UPPERCASE) between indicated strings as chars
		/// </summary>
		/// <param name="startLetterString" />
		/// <param name="endLetterString" />
		/// <returns />
		public static List<char> GetLetterList(string startLetterString, string endLetterString)
		{
			char startLetter = Convert.ToChar(startLetterString);
			char endLetter = Convert.ToChar(endLetterString);

			return GetLetterList(startLetter, endLetter);
		}

		/// <summary>
		/// Normalizes string as Filename
		/// </summary>
		/// <param name="fileName" />
		/// <returns>Normalized Filename</returns>
		public static string NormalizeFileName(this string fileName)
		{
			return fileName.ToLower();
		}

		/// <summary>
		/// Returns string with first char lowercase
		/// </summary>
		/// <param name="value" />
		/// <returns>String with first char lowercase</returns>
		public static string FirstCharLower(this string value)
		{
			return value.IsNullOrWhiteSpace()
				       ? string.Empty
				       : string.Format("{0}{1}", char.ToLower(value[0]), value.Substring(1));
		}

		/// <summary>
		/// Returns string with first char uppercase
		/// </summary>
		/// <param name="value" />
		/// <returns>String with first char uppercase</returns>
		public static string FirstCharUpper(this string value)
		{
			return value.IsNullOrWhiteSpace()
				       ? string.Empty
				       : string.Format("{0}{1}", char.ToUpper(value[0]), value.Substring(1));
		}

		/// <summary>
		/// Pads a string making it always the provided length. Trims if string is too long.
		/// </summary>
		/// <param name="value">String to pad/trim.</param>
		/// <param name="length">Length the string should be.</param>
		/// <returns>String containing pads, if it is longer than requested length it will be trimmed.</returns>
		public static string FixLengthString(this string value, int length)
		{
			const string padding = " ";

			if (string.IsNullOrEmpty(value))
			{
				StringBuilder builder = new StringBuilder(length);

				for (int i = 0; i < length; i++)
				{
					builder.Append(padding);
				}

				return builder.ToString();
			}

			if (value.Length < length)
			{
				StringBuilder builder = new StringBuilder(length);
				builder.Append(value);

				int paddingCount = length - value.Length;

				for (int i = 0; i < paddingCount; i++)
				{
					builder.Append(padding);
				}

				return builder.ToString();
			}

			return value.Length > length ? value.Substring(0, length) : value;
		}

		/// <summary>
		/// Pads a number with 0 making it always the provided length. Trims if number is too long.
		/// </summary>
		/// <param name="value">Number (as a string) to pad/trim.</param>
		/// <param name="length">Length the string should be.</param>
		/// <returns>String containing pads, if it is longer than requested length it will be trimmed.</returns>
		public static string FixLengthNumber(this string value, int length)
		{
			const string padding = "0";

			if (string.IsNullOrEmpty(value))
			{
				StringBuilder builder = new StringBuilder(length);

				for (int i = 0; i < length; i++)
				{
					builder.Append(padding);
				}

				return builder.ToString();
			}

			if (value.Length < length)
			{
				StringBuilder builder = new StringBuilder(length);
				builder.Append(value);

				int paddingCount = length - value.Length;

				for (int i = 0; i < paddingCount; i++)
				{
					builder.Insert(0, padding);
				}

				return builder.ToString();
			}

			return value.Length > length ? value.Substring(0, length) : value;
		}

		/// <summary>
		/// If value is not set, return valueToUse. Otherwise, return value.
		/// </summary>
		/// <param name="value" />
		/// <param name="valueToUse" />
		/// <returns>ValueToUse, if Value is NULL or WhiteSpace. Otherwise, Value.</returns>
		public static string GetValueOrDefault(this string value, string valueToUse)
		{
			return value.IsNullOrWhiteSpace() ? valueToUse.TrimSafely() : value.TrimSafely();
		}

		/// <summary>
		/// Safely replaces valueToReplace with valueToReplaceWith in value
		/// </summary>
		/// <param name="value" />
		/// <param name="valueToReplace" />
		/// <param name="valueToReplaceWith" />
		/// <returns />
		public static string SafeReplace(this string value, string valueToReplace, string valueToReplaceWith)
		{
			return value.IsNullOrWhiteSpace()
				       ? value
				       : value.Replace(valueToReplace, valueToReplaceWith);
		}

		/// <summary>
		/// Determines whether a string is not null or whitespace.
		/// </summary>
		/// <param name="value" />
		/// <returns>True, if this string is not empty/whitespace. Otherwise, false.</returns>
		public static bool HasValue(this string value)
		{
			return !IsNullOrWhiteSpace(value);
		}

        /// <summary>
        /// Gets the last specified number of characters from this string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="numberOfDigitsToGet"></param>
        /// <returns>A String</returns>
        public static string GetLast(this string s, int numberOfDigitsToGet)
        {
            string temp = string.Empty;

            for (int i = s.Length - 1; i >= 0 && numberOfDigitsToGet > 0; i --)
            {
                temp = temp.Append(s[i].ToString(CultureInfo.InvariantCulture));

                numberOfDigitsToGet--;
            }

            return temp.Reverse();
        }

        /// <summary>
        /// Splits a string by a specified character
        /// </summary>
        /// <param name="s"></param>
        /// <param name="delimiter"></param>
        /// <param name="trimElements"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string[] Split(this string s, 
                                     string delimiter,
                                     bool trimElements = true,
                                     StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            string[] elements = s.Split(new[] {delimiter}, options);

            if (trimElements)
            {
                elements = elements.Select(dd => TrimSafely(dd)).ToArray();
            }

            return elements;
        }

	    /// <summary>
	    /// Returns this string encoded as an array of bytes using the default system encoding
	    /// </summary>
	    /// <param name="s"></param>
	    /// <returns></returns>
	    public static byte[] ToByteArray(this string s)
	    {
	        return Encoding.Default.GetBytes(s);
	    }

		#endregion

		#region StringBuilder Methods

		/// <summary>
		/// Appends the specified text as a new line using an HTML break tag.
		/// </summary>
		/// <param name="builder" />
		/// <param name="text" />
		/// <returns>A StringBuilder with the text appended as an html line</returns>
		public static void AppendHtmlLine(this StringBuilder builder, string text)
		{
			if (!text.IsNullOrWhiteSpace())
			{
				builder.AppendFormat("{0}{1}", text, BreakTag);
			}
		}

	    /// <summary>
	    /// Appends a copy of the specified text as a composite format, then appends the default environment line break character.
	    /// </summary>
	    /// <param name="builder"></param>
	    /// <param name="text"></param>
	    /// <param name="isHtml"></param>
	    /// <param name="args"></param>
	    public static void AppendFormatLine(this StringBuilder builder, string text, bool isHtml = false, params object[] args)
	    {
	        if (!text.IsNullOrWhiteSpace())
	        {
	            text += EnvironmentNewLine(isHtml);

	            builder.AppendFormat(text, args);
	        }
	    }

		/// <summary>
		/// Appends text with a line break at the end
		/// </summary>
		/// <param name="builder" />
		/// <param name="isHtml" />
		/// <param name="text" />
		/// <returns />
		public static void AppendLine(this StringBuilder builder, string text, bool isHtml)
		{
			if (isHtml)
			{
				AppendHtmlLine(builder, text);
			}
			else
			{
				builder.AppendLine(text);
			}
		}

		/// <summary>
		/// Checks if the string contained in this StringBuilder is null or white space
		/// </summary>
		/// <param name="builder" />
		/// <returns />
		public static bool IsNullOrWhiteSpace(this StringBuilder builder)
		{
			return builder != null && builder.ToString().IsNullOrWhiteSpace();
		}

		/// <summary>
		/// Removes extra spaces from StringBuilder
		/// </summary>
		/// <param name="builder" />
		/// <returns />
		public static string RemoveExtraSpaces(this StringBuilder builder)
		{
			return builder == null ? string.Empty : builder.ToString().RemoveExtraSpaces();
		}

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="xml"></param>
	    /// <param name="outerElement"></param>
	    /// <returns></returns>
	    public static DataSet ToDataSet(this string xml, string outerElement)
	    {
            DataSet _ds = new DataSet();
            DataTable _dt = new DataTable("serial");

            _dt.Columns.Add("name", typeof(string));
            _dt.Columns.Add("value", typeof(string));
            _ds.Tables.Add(_dt);

            string _xml = xml.Replace("<" + outerElement + ">", string.Empty).Replace("</" + outerElement + ">", string.Empty);

            XmlDocument xdoc = new XmlDocument();

            xdoc.LoadXml(_xml);

            string _name = string.Empty;
            string _value = string.Empty;

            foreach (XmlNode xn in xdoc["serial"].ChildNodes)
            {
                if (xn.Name == "name")
                {
                    _name = xn.InnerText;
                }

                if (xn.Name == "value")
                {
                    _value = (xn.InnerText == string.Empty ? "-" : xn.InnerText);
                }

                if ((string.IsNullOrEmpty(_name) == false) && (string.IsNullOrEmpty(_value) == false))
                {
                    DataRow _dr = _dt.NewRow();

                    _dr["name"] = _name;
                    _dr["value"] = (_value == "-" ? string.Empty : _value);

                    _name = string.Empty;
                    _value = string.Empty;

                    _dt.Rows.Add(_dr);
                }
            }

            return _ds;
	    }

		#endregion

		#region Char Methods

		/// <summary>
		/// Returns a list of all chars (UPPERCASE) between indicated chars
		/// </summary>
		/// <param name="startLetter" />
		/// <param name="endLetter" />
		/// <returns />
		public static List<char> GetLetterList(char startLetter, char endLetter)
		{
			List<char> letterList = new List<char>();

			startLetter = char.ToUpper(startLetter);
			endLetter = char.ToUpper(endLetter);

			for (char i = startLetter; i <= endLetter; i++)
			{
				letterList.Add(i);
			}

			return letterList;
		}

		#endregion

		#region RegEx Methods
		/// <summary>
		/// Validate Email Address
		/// </summary>
		/// <param name="input" />
		/// <returns>True, if input is valid email</returns>
		public static bool IsEmail(this string input)
		{
			const string regex = "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";
			return ValidateInput(input, regex);
		}

		/// <summary>
		/// Validate Numeric
		/// </summary>
		/// <param name="input" />
		/// <returns>True, if input is numeric</returns>
		public static bool IsNumeric(this string input)
		{
			const string regex = "^[0-9]+$";
			return ValidateInput(input, regex);
		}

	    /// <summary>
	    /// Validate Alpha
	    /// </summary>
	    /// <param name="input"></param>
	    /// <returns>True if input is alpha</returns>
	    public static bool IsAlpha(this string input)
	    {
	        return !IsNumeric(input);
	    }

	    /// <summary>
	    /// Validate SSN
	    /// </summary>
	    /// <param name="input" />
	    /// <param name="allowDashes"></param>
	    /// <returns>True, if input is valid SSN</returns>
	    public static bool IsValidSSN(this string input, bool allowDashes)
		{
			return ValidateInput(input, allowDashes ? @"^\d{3}-\d{2}-\d{4}$" : @"^\d{9}$");
		}

		/// <summary>
		/// Helper Method to validate input against regular expression
		/// </summary>
		/// <param name="input">String to validate</param>
		/// <param name="regex">Regular expression to validate against</param>
		/// <returns>True, if input is valid against regular expression</returns>
		private static bool ValidateInput(string input, string regex)
		{
			return !input.IsNullOrWhiteSpace() && Regex.IsMatch(input, regex);
		}
		#endregion
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SflStucco.Site.Helpers
{
    public static class StringExtensions
    {
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
        /// Checks if a string is null or empty
        /// </summary>
        /// <param name="value" />
        /// <returns>True if null or empty, false otherwise</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
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
        /// Trims a string without the risk of null exception
        /// </summary>
        /// <param name="value">String to trim</param>
        /// <param name="delims">Delimiters to trim</param>
        /// <returns>Safely trimmed string</returns>
        public static string TrimSafely(this string value, params char[] delims)
        {
            if (delims == null)
            {
                delims = new[] { ' ' };
            }

            return String.IsNullOrEmpty(value) ? value : value.Trim(delims);
        }

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
    }
}
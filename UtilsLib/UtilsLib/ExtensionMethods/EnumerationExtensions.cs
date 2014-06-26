using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilsLib.ExtensionMethods
{
    [Serializable]
    public static class EnumerationsHelper
    {
        /// <summary>
        /// Helper method that safely converts an integer into an Enum
        /// </summary>
        /// <param name="enumValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ConvertFromInteger<T>(int enumValue)
        {
            T returnVal = default(T);

            Type baseType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            foreach (T val in Enum.GetValues(baseType).Cast<T>().ToList().Where(val => Convert.ToInt32(val) == enumValue))
            {
                returnVal = val;
            }

            return returnVal;
        }

        /// <summary>
        /// Helper method that safely converts a string into an Enum
        /// </summary>
        /// <param name="enumValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ConvertFromString<T>(string enumValue)
        {
            enumValue = enumValue.TrimSafely();

            T defaultItem = default(T);

            Type baseType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            foreach (Enum item in Enum.GetValues(baseType))
            {
                if (enumValue.SafeEquals(item.ToString(), StringComparison.CurrentCultureIgnoreCase) ||
                    enumValue.SafeEquals(item.ToFormattedString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    object foundItem = item;
                    return (T)foundItem;
                }
            }
            return defaultItem;
        }

        /// <summary>
        /// Gets Enumeration Values
        /// </summary>
        /// <typeparam name="T" />
        /// <param name="removeDefaults">If true, will remove all Enums with values less than or equal to 0</param>
        /// <param name="sortByName">If true, will sort by name rather than by value</param>
        /// <returns>List of Enumeration Values</returns>
        public static List<T> GetEnumerationValues<T>(bool removeDefaults = false, bool sortByName = false)
        {
            Type baseType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            List<T> enumValues = Enum.GetValues(baseType).Cast<T>().ToList();

            try
            {
                if (removeDefaults)
                {
                    enumValues.RemoveAll(e => Convert.ToInt32(e) <= 0);
                }
            }
            catch { }

            return sortByName ? enumValues.OrderBy(e => e.ToString()).ToList() : enumValues;
        }

        /// <summary>
        /// Gets the specified values for an enumeration as a delimited string with a comma as the delimiter
        /// </summary>
        /// <param name="enumerationValues">If specified, will limit the enumeration values to those.</param>
        /// <param name="delimiter">If specified, will use as delimiter. Defaults to comma.</param>
        /// <returns>A string</returns>
        public static string GetEnumerationValuesAsDelimitedString<T>(IEnumerable<T> enumerationValues = null, string delimiter = ",")
        {
            if (enumerationValues == null)
            {
                enumerationValues = GetEnumerationValues<T>();
            }

            StringBuilder builder = new StringBuilder();
            foreach (T enumerationValue in enumerationValues)
            {
                builder.AppendFormat("{0}{1}", Convert.ToInt32(enumerationValue), delimiter);
            }

            return builder.ToString();
        }
    }

    /// <summary>
    /// Encapsulates all logic for Enumeration extension methods
    /// </summary>
    public static class EnumerationExtensions
    {
       
        /// <summary>
        /// Returns the enumeration value as a different formatted string
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string ToFormattedString(this Enum enumValue)
        {
            Type underlyingType = enumValue.GetType();

            return enumValue.ToString();
        }
    }
}

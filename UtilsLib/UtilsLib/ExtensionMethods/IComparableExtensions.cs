using System;
using System.Linq;

namespace UtilsLib.ExtensionMethods
{
    /// <summary>
    /// Extension methods for IComparable
    /// </summary>
    public static class IComparableExtensions
    {
        // ReSharper disable LoopCanBePartlyConvertedToQuery
        /// <summary>
        /// Returns minimum value of an array of values
        /// </summary>
        /// <param name="value" />
        /// <typeparam name="T" />
        /// <returns />
        public static T Minimum<T>(params T[] value) where T : IComparable<T>
        {
            T min = value.FirstOrDefault();
            foreach (T obj in value)
            {
                if (obj.CompareTo(min) < 0)
                {
                    min = obj;
                }
            }
            return min;
        }

        /// <summary>
        /// Returns maximum value of an array of values
        /// </summary>
        /// <param name="value" />
        /// <typeparam name="T" />
        /// <returns />
        public static T Maximum<T>(params T[] value) where T : IComparable<T>
        {
            T min = value.FirstOrDefault();
            foreach (T obj in value)
            {
                if (obj.CompareTo(min) > 0)
                {
                    min = obj;
                }
            }
            return min;
        }
        // ReSharper restore LoopCanBePartlyConvertedToQuery
    }
}

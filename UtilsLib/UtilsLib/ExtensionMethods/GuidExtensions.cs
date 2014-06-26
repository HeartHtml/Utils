using System;

namespace UtilsLib.ExtensionMethods
{
    /// <summary>
    /// Extension Methods for Guid
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// Checks if a guid is empty, new but improperly initialized
        /// </summary>
        /// <param name="value" />
        /// <returns>True if empty or initialized, false otherwise</returns>
        public static bool IsNullOrDefault(this Guid value)
        {
            return value == default(Guid) || value == Guid.Empty || value == new Guid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid? GetNullIfDefault(this Guid value)
        {
            return value == new Guid() || value == default(Guid) ? (Guid?) null : value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrDefault(this Guid? value) 
        {
            return !value.HasValue || (value.Value == default(Guid) || value.Value == Guid.Empty || value.Value == new Guid());
        }
    }
}
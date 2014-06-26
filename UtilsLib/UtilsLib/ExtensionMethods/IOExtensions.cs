using System.IO;
using System.Linq;

namespace UtilsLib.ExtensionMethods
{
    /// <summary>
    /// Extension Methods for IO Classes
    /// </summary>
    public static class IOExtensions
    {
        /// <summary>
        /// Simulates the .NET 4.0 Path.Combine,
        /// which allows for params of string
        /// </summary>
        /// <param name="paths">Paths to combine</param>
        /// <returns>Combined Path</returns>
        public static string PathCombine(params string[] paths)
        {
            string combinedPath = string.Empty;

            if (paths.SafeAny())
            {
                combinedPath = paths.Aggregate(combinedPath, Path.Combine);
            }

            return combinedPath;
        }
    }
}

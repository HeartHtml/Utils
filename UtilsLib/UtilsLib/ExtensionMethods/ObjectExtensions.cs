using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace UtilsLib.ExtensionMethods
{
    /// <summary>
    /// Extension Methods for object class
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Determines if this object is equal to its internal default value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True if default, false otherwise</returns>
        public static bool IsDefault<T>(this T value)
        {
            if (value == null)
            {
                return true;
            }

            T defaultValue = default(T);

            return value.Equals(defaultValue);
        }

        /// <summary>
        /// Safely converts this object to a string representation or null
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>A string</returns>
        public static string ToSafeString<T>(this T value)
        {
            if (value == null)
            {
                return null;
            }

            return value.ToString();
        }

        /// <summary>
        /// Returns Byte[] representation of T
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Byte[]</returns>
        public static byte[] ToBinary<T>(this T value)
        {
            byte[] binary;

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter bFormatter = new BinaryFormatter();

                stream.Position = 0;

                bFormatter.Serialize(stream, value);

                stream.Position = 0;

                using (BinaryReader br = new BinaryReader(stream))
                {
                    binary = br.ReadBytes((int)stream.Length);
                }
            }

            return binary;
        }


        /// <summary>
        /// Gets an Object from Serialized Byte Stream
        /// </summary>
        /// <param name="value">The byte enumerable to Deserialize</param>
        /// <returns />
        public static object ToObjectFromBinary(this IEnumerable<byte> value)
        {
            object obj;

            using (MemoryStream stream = new MemoryStream(value.ToArray()))
            {
                BinaryFormatter bFormatter = new BinaryFormatter();

                stream.Position = 0;

                obj = bFormatter.Deserialize(stream);
            }

            return obj;
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}

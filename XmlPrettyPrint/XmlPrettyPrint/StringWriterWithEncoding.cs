using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XmlPrettyPrint
{
    /// <summary>
    /// Encapsulates all logic for StringWriterWithEncoding
    /// </summary>
    public sealed class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding encoding;

        /// <summary>
        /// Specifies a string writer with the specified encoding
        /// </summary>
        /// <param name="encoding"></param>
        public StringWriterWithEncoding(Encoding encoding)
        {
            this.encoding = encoding;
        }

        /// <summary>
        /// Gets the encoding of this StringWriter
        /// </summary>
        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }
}

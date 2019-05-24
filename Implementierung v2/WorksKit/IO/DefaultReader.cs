using System;
using System.IO;
using System.Text;

namespace WorksKit.IO
{
    public class DefaultReader : BinaryReader
    {
        public DefaultReader(Stream input) : base(input)
        {
        }

        public DefaultReader(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public Guid ReadGuid()
        {
            return new Guid(ReadBytes(16));
        }
    }
}
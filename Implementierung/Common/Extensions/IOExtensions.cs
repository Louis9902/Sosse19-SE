using System;
using System.IO;

namespace Backupper.Common.Extensions
{
    public static class IOExtensions
    {
        public static Guid ReadGuid(this BinaryReader reader)
        {
            return new Guid(reader.ReadBytes(16));
        }

        public static void Write(this BinaryWriter writer, Guid guid)
        {
            writer.Write(guid.ToByteArray());
        }
    }
}
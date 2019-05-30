using System;
using System.IO;

namespace WorksKit.Utilities
{
    /// <summary>
    /// ActuallyAdditions provides some extensions for quality of live operations
    /// </summary>
    public static class ActuallyAdditions
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
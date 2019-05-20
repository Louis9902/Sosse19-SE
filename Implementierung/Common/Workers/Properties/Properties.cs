using System.Collections.Generic;
using System.IO;

namespace Backupper.Common
{
    public class Properties : IExternalizable
    {
        private readonly Dictionary<string, Property> properties;

        public Properties()
        {
            properties = new Dictionary<string, Property>();
        }

        public object this[string name]
        {
            get => properties[name];
        }

        public void SaveExternal(BinaryWriter writer)
        {
            writer.Write(properties.Count);

            foreach (var property in properties)
            {
                writer.Write(property.Key);
                property.Value.SaveExternal(writer);
            }
        }

        public void LoadExternal(BinaryReader reader)
        {
            var count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                var name = reader.ReadString();
            }
        }
    }
}
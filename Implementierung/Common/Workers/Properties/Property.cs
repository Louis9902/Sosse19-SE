using System.IO;

namespace Backupper.Common
{
    public abstract class Property : IExternalizable
    {
        public string Name { get; }
        public object Value { get; set; }

        public Property(string name)
        {
            Name = name;
        }

        public abstract void SaveExternal(BinaryWriter writer);

        public abstract void LoadExternal(BinaryReader reader);

        public static explicit operator string(Property instance)
        {
            return instance.Value is string value ? value : instance.Value.ToString();
        }
    }

    
}
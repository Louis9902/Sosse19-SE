using System.Collections.Generic;
using System.IO;

namespace Backupper.Common.Workers.Properties
{
    public class PropertyMap 
    {
        private readonly Dictionary<string, Property> properties;

        public PropertyMap()
        {
            properties = new Dictionary<string, Property>();
        }

        public Property this[string name]
        {
            get => properties[name];
            set => properties[name] = value;
        }
    }
}
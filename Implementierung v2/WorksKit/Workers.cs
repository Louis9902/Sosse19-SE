using System;
using System.Collections.Generic;
using System.IO;
using WorksKit.IO;
using WorksKit.Worker;

namespace WorksKit
{
    public class Workers
    {
        private string Configuration { get; }

        public Workers(string configuration)
        {
            Configuration = configuration;
        }

        public bool Load(IDictionary<Guid, IWorker> workers)
        {
            using (var stream = new FileStream(Configuration, FileMode.Open, FileAccess.Read))
            {
                var reader = new DefaultReader(stream);
                var count = reader.ReadInt32();

                for (var i = 0; i < count; i++)
                {
                    var group = reader.ReadGuid();
                    var label = reader.ReadGuid();

                    if (!WorkerTypes.Bindings.GetOrNothing(group, out var clazz))
                    {
                        continue;
                    }

                    var worker = BasicWorker.New(clazz, group, label);
                }
            }

            return false;
        }

        public bool Save(IDictionary<Guid, IWorker> workers)
        {
            using (var stream = new FileStream(Configuration, FileMode.Create, FileAccess.Write))
            {
                var writer = new BinaryWriter(stream);
            }

            return false;
        }
    }
}
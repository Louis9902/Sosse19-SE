using System;
using System.Collections.Generic;
using System.IO;
using WorksKit.Worker;

namespace WorksKit
{
    public class Workers
    {
        private string Configuration { get; }

        public bool Load(IDictionary<Guid, IWorker> workers)
        {
            using (var stream = new FileStream(Configuration, FileMode.Open, FileAccess.Read))
            {
                var reader = new BinaryReader(stream);
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
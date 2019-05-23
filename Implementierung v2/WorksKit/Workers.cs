using System;
using System.Collections.Generic;
using WorksKit.Worker;

namespace WorksKit
{
    public class Workers
    {
        private string Configuration { get; }

        public bool Load(IDictionary<Guid, IWorker> workers)
        {
            return false;
        }

        public bool Save(IDictionary<Guid, IWorker> workers)
        {
            return false;
        }
    }
}
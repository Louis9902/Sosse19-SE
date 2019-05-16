using System;
using System.IO;
using System.Threading;

namespace Backupper.Core.Tasks
{
    public abstract class Worker
    {
        private static readonly ThreadLocal<WorkerInfo> Info = new ThreadLocal<WorkerInfo>();

        public Guid Group { get; }
        public Guid Identifier { get; }

        protected Worker()
        {
            Group = Info.Value.Group;
            Identifier = Info.Value.Identifier;
        }

        public static Worker NewInstance(Type clazz, Guid group, Guid identifier)
        {
            Info.Value = new WorkerInfo {Group = group, Identifier = identifier};
            var worker = Activator.CreateInstance(clazz);
            Info.Value = null;
            return (Worker) worker;
        }

        public abstract bool Load(Stream stream);

        public abstract void Start();

        public abstract bool Save(Stream stream);

        public abstract void Abort();

        private class WorkerInfo
        {
            public Guid Group { get; set; }
            public Guid Identifier { get; set; }
        }
    }
}
using System;
using System.IO;
using System.Threading;

namespace Backupper.Common
{
    public abstract class BasicWorker : IWorker
    {
        private static readonly ThreadLocal<WorkerInfo> Info = new ThreadLocal<WorkerInfo>();

        public Guid Group { get; }
        public Guid Identifier { get; }

        protected BasicWorker()
        {
            Group = Info.Value.Group;
            Identifier = Info.Value.Identifier;
        }

        public static IWorker NewInstance(Type clazz, Guid group, Guid identifier)
        {
            Info.Value = new WorkerInfo {Group = group, Identifier = identifier};
            var worker = Activator.CreateInstance(clazz);
            Info.Value = null;
            return (IWorker) worker;
        }

        private class WorkerInfo
        {
            public Guid Group { get; set; }
            public Guid Identifier { get; set; }
        }

        public bool LoadData(Stream stream)
        {
            return true;
        }

        public bool SaveData(Stream stream)
        {
            return true;
        }

        public abstract void Start();

        public abstract void Abort();
    }
}
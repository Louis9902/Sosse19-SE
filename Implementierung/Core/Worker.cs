using System;
using System.IO;
using System.Threading;

namespace Backupper.Core
{
    public abstract class Worker
    {

        private static readonly ThreadLocal<WorkerInfo> info = new ThreadLocal<WorkerInfo>();

        private readonly WorkerManager manager;
        public Guid Identifier { get; }

        protected Worker()
        {
            var args = info.Value;
            manager = args.Manager;
            Identifier = args.Identifier;
        }

        public static Worker NewInstance(Type kind, WorkerManager workerManager, Guid identifier)
        {
            info.Value = new WorkerInfo { Manager = workerManager, Identifier = identifier };
            var worker = Activator.CreateInstance(kind);
            info.Value = null;
            return (Worker) worker;
        }

        public abstract bool Start(Stream stream);

        public abstract bool Abort(Stream stream);

        private class WorkerInfo
        {
            public WorkerManager Manager { get; set; }
            public Guid Identifier { get; set; }
        }

    }

}
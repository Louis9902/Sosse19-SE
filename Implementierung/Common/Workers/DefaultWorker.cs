using System;
using System.Threading;
using Backupper.Common.Workers.Properties;

namespace Backupper.Common.Workers
{
    public abstract class DefaultWorker : IWorker
    {
        private static readonly ThreadLocal<WorkerInfo> Info = new ThreadLocal<WorkerInfo>();

        public Guid Group { get; }
        public Guid Label { get; }

        public PropertyMap Properties { get; } = new PropertyMap();

        protected DefaultWorker()
        {
            Group = Info.Value.Group;
            Label = Info.Value.Label;
        }

        public static T NewInstance<T>(Guid group, Guid label) where T : IWorker
        {
            Info.Value = new WorkerInfo {Group = group, Label = label};
            var worker = Activator.CreateInstance<T>();
            Info.Value = null;
            return worker;
        }

        public static IWorker NewInstance(Guid group, Guid label, Type type)
        {
            Info.Value = new WorkerInfo {Group = group, Label = label};
            var worker = Activator.CreateInstance(type);
            Info.Value = null;
            return (IWorker) worker;
        }

        private class WorkerInfo
        {
            public Guid Group { get; set; }
            public Guid Label { get; set; }
        }

        public abstract void StartWorker();

        public abstract void AbortWorker();
    }
}
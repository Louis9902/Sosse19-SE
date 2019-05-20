using System;
using System.IO;
using System.Threading;
using Backupper.Common.Extensions;

namespace Backupper.Common
{
    public abstract class DefaultWorker : IWorker
    {
        private static readonly ThreadLocal<WorkerInfo> Info = new ThreadLocal<WorkerInfo>();

        public Guid Group { get; }
        public Guid Label { get; }

        public Properties Properties { get; } = new Properties();

        protected DefaultWorker()
        {
            Group = Info.Value.Group;
            Label = Info.Value.Label;
        }

        public void SaveExternal(BinaryWriter writer)
        {
            Properties.SaveExternal(writer);
        }

        public void LoadExternal(BinaryReader reader)
        {
            Properties.LoadExternal(reader);
        }

        public static T Create<T>(WorkerRegistry registry, Action<T> action = null) where T : IWorker
        {
            if (!registry.TryGet(typeof(T), out var group))
            {
                throw new ArgumentException($"Unable to find group identifier for {typeof(T)}");
            }

            var worker = NewInstance<T>(group, Guid.NewGuid());
            action?.Invoke(worker);
            return worker;
        }

        public static T NewInstance<T>(Guid group, Guid label) where T : IWorker
        {
            Info.Value = new WorkerInfo {Group = group, Label = label};
            var worker = Activator.CreateInstance<T>();
            Info.Value = null;
            return worker;
        }

        public static IWorker NewInstance(Guid group, Guid label, Type clazz)
        {
            Info.Value = new WorkerInfo {Group = group, Label = label};
            var worker = Activator.CreateInstance(clazz);
            Info.Value = null;
            return (IWorker) worker;
        }

        private class WorkerInfo
        {
            public Guid Group { get; set; }
            public Guid Label { get; set; }
        }

        public abstract void Start();

        public abstract void Abort();
    }
}
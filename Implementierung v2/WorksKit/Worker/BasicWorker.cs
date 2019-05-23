using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace WorksKit.Worker
{
    public abstract class BasicWorker : IWorker
    {
        private static readonly ThreadLocal<WorkerInfo> Info = new ThreadLocal<WorkerInfo>();
        private static readonly IFormatter Formatter = new BinaryFormatter();

        public Guid Group { get; }
        public Guid Label { get; }

        public Preferences Preferences { get; } = new Preferences();

        protected BasicWorker()
        {
            Group = Info.Value.Group;
            Label = Info.Value.Label;
        }

        public static T New<T>(Guid group, Guid label) where T : IWorker
        {
            Info.Value = new WorkerInfo {Group = group, Label = label};
            var worker = Activator.CreateInstance<T>();
            Info.Value = null;
            return worker;
        }

        public static IWorker New(Type type, Guid group, Guid label)
        {
            Info.Value = new WorkerInfo {Group = group, Label = label};
            var worker = Activator.CreateInstance(type);
            Info.Value = null;
            return (IWorker) worker;
        }

        public abstract void StartWorker();

        public abstract void AbortWorker();

        private class WorkerInfo
        {
            public Guid Group { get; set; }
            public Guid Label { get; set; }
        }
    }
}
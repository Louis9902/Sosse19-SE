using System;
using System.IO;
using System.Threading;
using TinyTasksKit.Worker.Preferences;

namespace TinyTasksKit.Worker
{
    public abstract class DefaultWorker : IWorker
    {
        private static readonly ThreadLocal<WorkerInfo> Info = new ThreadLocal<WorkerInfo>();

        protected DefaultWorker()
        {
            Group = Info.Value.Group;
            Label = Info.Value.Label;
        }

        public Guid Group { get; }
        public Guid Label { get; }

        public PreferenceSet Preferences { get; } = new PreferenceSet();

        public static T Instantiate<T>(Guid group, Guid label) where T : IWorker
        {
            Info.Value = new WorkerInfo {Group = group, Label = label};
            var worker = Activator.CreateInstance<T>();
            Info.Value = null;
            return worker;
        }

        public static IWorker Instantiate(Type type, Guid group, Guid label)
        {
            Info.Value = new WorkerInfo {Group = group, Label = label};
            var worker = Activator.CreateInstance(type);
            Info.Value = null;
            return (IWorker) worker;
        }

        public abstract void StartWorker();

        public abstract void AbortWorker();

        public void LoadPreferences(Stream stream)
        {
            Preferences.Load(stream);
        }

        public void SavePreferences(Stream stream)
        {
            Preferences.Save(stream);
        }

        private class WorkerInfo
        {
            public Guid Group { get; set; }
            public Guid Label { get; set; }
        }
    }
}
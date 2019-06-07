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

        /// <summary>
        /// Creates a new worker instance.
        /// Note: This is important to use instead of the new operator, as it will not inject the required parameters.
        /// </summary>
        /// <param name="group">The group id of the new worker object</param>
        /// <param name="label">The unique id of the new worker object</param>
        /// <typeparam name="T">The type of the new IWorker instance</typeparam>
        /// <returns>The new instantiated object</returns>
        /// <seealso cref="Instantiate"/>
        /// <seealso cref="Workers.CreateWorker{T}"/>
        public static T Instantiate<T>(Guid group, Guid label) where T : IWorker
        {
            Info.Value = new WorkerInfo {Group = group, Label = label};
            var worker = Activator.CreateInstance<T>();
            Info.Value = null;
            return worker;
        }

        /// <summary>
        /// Creates a new worker instance.
        /// Note: This is important to use instead of the new operator, as it will not inject the required parameters.
        /// </summary>
        /// <param name="type">The type of the new IWorker instance</param>
        /// <param name="group">The group id of the new worker object</param>
        /// <param name="label">The unique id of the new worker object</param>
        /// <returns>The new instantiated object</returns>
        /// <seealso cref="Instantiate{T}"/>
        /// <seealso cref="Workers.CreateWorker{T}"/>
        public static IWorker Instantiate(Type type, Guid group, Guid label)
        {
            Info.Value = new WorkerInfo {Group = group, Label = label};
            var worker = Activator.CreateInstance(type);
            Info.Value = null;
            return (IWorker) worker;
        }

        public abstract void StartWorker();

        public abstract void AbortWorker();

        /// <summary>
        /// Loads the preferences from the stream into the current context.
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        public void LoadPreferences(Stream stream)
        {
            Preferences.Load(stream);
        }

        /// <summary>
        /// Saves the preferences from the current context onto the stream.
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        public void SavePreferences(Stream stream)
        {
            Preferences.Save(stream);
        }

        /// <summary>
        /// Holder for parameter injection into the instance.
        /// </summary>
        /// <seealso cref="DefaultWorker.Info"/>
        private class WorkerInfo
        {
            public Guid Group { get; set; }
            public Guid Label { get; set; }
        }
    }
}
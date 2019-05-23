using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using WorksKit.IO;

namespace WorksKit.Worker
{
    public class BasicWorker : IWorker, IExternalizable
    {
        private static readonly ThreadLocal<WorkerInfo> Info = new ThreadLocal<WorkerInfo>();
        private static readonly IFormatter Formatter = new BinaryFormatter();

        public Guid Group { get; }
        public Guid Label { get; }

        private readonly IDictionary parameters; // property object cache
        private readonly IDictionary properties; // property values cache

        protected BasicWorker()
        {
            Group = Info.Value.Group;
            Label = Info.Value.Label;

            parameters = new Hashtable();
            properties = Info.Value.Properties;
        }

        public static T NewInstance<T>(Guid group, Guid label) where T : IWorker
        {
            Info.Value = new WorkerInfo {Group = group, Label = label, Properties = new Hashtable()};
            var worker = Activator.CreateInstance<T>();
            Info.Value = null;
            return worker;
        }

        public static IWorker NewInstance(Type type, Guid group, Guid label)
        {
            Info.Value = new WorkerInfo {Group = group, Label = label, Properties = new Hashtable()};
            var worker = Activator.CreateInstance(type);
            Info.Value = null;
            return (IWorker) worker;
        }

        protected Property<T> Property<T>(string name, T value = default, T fallback = default)
        {
            var result = parameters[name] ?? (parameters[name] = new Property<T>(properties, name, value, fallback));
            return (Property<T>) result;
        }

        public void LoadExternal(Stream stream)
        {
            // ToDo: load internal properties
        }

        public void SaveExternal(Stream stream)
        {
            // ToDo: save internal properties
        }

        private class WorkerInfo
        {
            public Guid Group { get; set; }
            public Guid Label { get; set; }
            public IDictionary Properties { get; set; }
        }
    }

    public class Property<T>
    {
        private readonly IDictionary indexer;

        public T Value
        {
            get => indexer[Name] is T result ? result : Fallback;
            set => indexer[Name] = value;
        }

        public T Fallback { get; }
        public string Name { get; }

        public Property(IDictionary index, string name, T value, T fallback)
        {
            indexer = index ?? throw new ArgumentNullException(nameof(index));
            Value = value;
            Fallback = fallback;
            Name = name;
        }

        public static implicit operator T(Property<T> property)
        {
            return property.Value;
        }
    }
}
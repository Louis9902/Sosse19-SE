using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WorksKit.IO;
using WorksKit.Worker;

namespace WorksKit
{
    public static class WorkerTypes
    {
        public static TypeBindings<Guid> Bindings { get; } = new TypeBindings<Guid>();

        static WorkerTypes()
        {
            var running = Assembly.GetExecutingAssembly();
            var calling = Assembly.GetCallingAssembly();

            SearchTypes(running);
            if (running != calling)
                SearchTypes(calling);
        }

        private static void SearchTypes(Assembly assembly)
        {
            var worker = typeof(IWorker);
            var result = from clazz in assembly.GetTypes().AsParallel()
                where worker.IsAssignableFrom(clazz)
                let workerType = clazz.GetCustomAttribute<WorkerType>()
                where workerType.Group != Guid.Empty
                select new KeyValuePair<Guid, Type>(workerType.Group, clazz);

            Bindings.Register(result);
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class WorkerType : Attribute
    {
        public readonly Guid Group;

        public WorkerType(string group)
        {
            Guid.TryParse(group, out Group);
        }
    }
}
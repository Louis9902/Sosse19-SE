using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WorksKit.Utilities;

namespace WorksKit.Worker.Group
{
    public class WorkerGroups
    {
        private static readonly Lazy<WorkerGroups> Groups = new Lazy<WorkerGroups>(() => new WorkerGroups());
        private static readonly Type WorkerType = typeof(IWorker);

        private readonly TypeObjectBindings<Guid> bindings = new TypeObjectBindings<Guid>();

        private WorkerGroups()
        {
            SearchGroups();
        }

        public static TypeObjectBindings<Guid> ObjectBindings => Groups.Value.bindings;

        private void SearchGroups()
        {
            SearchTypes(WorkerType.Assembly);
        }

        private void SearchTypes(Assembly assembly)
        {
            var result = from clazz in assembly.GetTypes().AsParallel()
                where WorkerType.IsAssignableFrom(clazz)
                let workerGroup = clazz.GetCustomAttribute<WorkerGroup>()
                where workerGroup != null && workerGroup.Group != Guid.Empty
                select new KeyValuePair<Guid, Type>(workerGroup.Group, clazz);

            bindings.Register(result);
        }
    }
}
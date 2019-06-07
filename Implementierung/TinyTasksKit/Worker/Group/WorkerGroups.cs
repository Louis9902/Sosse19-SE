using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TinyTasksKit.Utilities;

namespace TinyTasksKit.Worker.Group
{
    /// <summary>
    /// Register for all IWorker types found in the assembly.
    /// This is computed when needed.
    /// This will scan the current assemblies and search for types that implement the IWorker interface
    /// and are annotated with the <see cref="WorkerGroup"/> attribute.
    /// This uses the <see cref="TypeObjectBindings{TBin}"/> for storing the found types.
    /// </summary>
    public class WorkerGroups
    {
        private static readonly Lazy<WorkerGroups> Groups = new Lazy<WorkerGroups>(() => new WorkerGroups());
        private static readonly Type WorkerType = typeof(IWorker);

        private readonly TypeObjectBindings<Guid> bindings = new TypeObjectBindings<Guid>();

        private WorkerGroups()
        {
            SearchGroups();
        }

        /// <summary>
        /// Accessor for the ObjectBindings.
        /// This will compute at first call.
        /// </summary>
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
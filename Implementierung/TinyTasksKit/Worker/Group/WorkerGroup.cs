using System;

namespace TinyTasksKit.Worker.Group
{
    /// <summary>
    /// Marker attribute for IWorker types. This also includes the group id for the worker.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class WorkerGroup : Attribute
    {
        public readonly Guid Group;

        public WorkerGroup(string group)
        {
            if (!Guid.TryParse(group, out Group))
            {
                throw new ArgumentException("invalid group guid");
            }
        }
    }
}
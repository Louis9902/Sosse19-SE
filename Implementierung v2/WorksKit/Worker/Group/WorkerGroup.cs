using System;

namespace WorksKit.Worker.Group
{
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
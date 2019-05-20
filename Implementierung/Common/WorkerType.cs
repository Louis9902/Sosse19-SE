using System;

namespace Backupper.Common
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class WorkerType : Attribute
    {
        public readonly Guid Identifier;

        public WorkerType(string identifier)
        {
            Guid.TryParse(identifier, out Identifier);
        }
    }
}
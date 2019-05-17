using System;

namespace Backupper.Common
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class WorkerType : Attribute
    {
        public WorkerType(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }
    }
}
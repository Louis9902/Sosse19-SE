using System;

namespace Backupper.Common
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class WorkerType : Attribute
    {
        public string Identifier { get; set; }
    }
}
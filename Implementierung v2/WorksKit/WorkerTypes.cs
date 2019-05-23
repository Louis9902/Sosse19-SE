using System;
using System.Reflection;

namespace WorksKit
{
    public static class WorkerTypes
    {
        
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
            
        }
        
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class WorkerType : Attribute
    {
        public readonly Guid Group;

        public WorkerType(string group)
        {
            Guid.TryParse(group, out Group);
        }
    }
}
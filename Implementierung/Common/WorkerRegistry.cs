using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Backupper.Common
{
    public class WorkerRegistry
    {
        private readonly IDictionary<Guid, Type> mapGuidToType = new Dictionary<Guid, Type>();
        private readonly IDictionary<Type, Guid> mapTypeToGuid = new Dictionary<Type, Guid>();

        public WorkerRegistry()
        {
            ScanAssembly(Assembly.GetExecutingAssembly());
        }
        
        public static explicit operator string(WorkerRegistry workers)
        {
            var builder = new StringBuilder();
            foreach (var pair in workers.mapGuidToType)
            {
                builder.AppendLine($"{pair.Value.Name}@{pair.Key}");
            }
            return builder.ToString();
        }

        public bool TryGet(Guid group, out Type clazz)
        {
            return mapGuidToType.TryGetValue(group, out clazz);
        }

        public bool TryGet(Type clazz, out Guid group)
        {
            return mapTypeToGuid.TryGetValue(clazz, out group);
        }

        //public static void LoadAllBinDirectoryAssemblies()
        //{
        //    string binPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"); // note: don't use CurrentEntryAssembly or anything like that.
        //
        //    foreach (string dll in Directory.GetFiles(binPath, "*.dll", SearchOption.AllDirectories))
        //    {
        //        try
        //        {                    
        //            Assembly loadedAssembly = Assembly.LoadFile(dll);
        //        }
        //        catch (FileLoadException loadEx)
        //        { } // The Assembly has already been loaded.
        //        catch (BadImageFormatException imgEx)
        //        { } // If a BadImageFormatException exception is thrown, the file is not an assembly.
        //
        //    } // foreach dll
        //}

        private void ScanAssembly(Assembly archive)
        {
            Guid GetIdentifier(string str)
            {
                Guid.TryParse(str, out var result);
                return result;
            }

            var worker = typeof(IWorker);
            var results = from clazz in archive.GetTypes().AsParallel()
                where worker.IsAssignableFrom(clazz)
                let annotation = clazz.GetCustomAttribute<WorkerType>()
                let identifier = GetIdentifier(annotation?.Identifier)
                where identifier != Guid.Empty
                select new {Identifier = identifier, Clazz = clazz};


            foreach (var result in results)
            {
                if (mapGuidToType.TryGetValue(result.Identifier, out var clazz))
                {
                    // ToDo: Handle double mapping of type {clazz} with id {Identifier}
                }

                mapGuidToType[result.Identifier] = result.Clazz;
                mapTypeToGuid[result.Clazz] = result.Identifier;
            }
        }
    }
}
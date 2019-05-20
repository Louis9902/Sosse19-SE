using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Backupper.Common.Extensions;
using Backupper.Common.IO;
using Backupper.Common.Workers;

namespace Backupper.Common
{
    public class WorkerConfigurator
    {
        private readonly TypeBindings<Guid> bindings = new TypeBindings<Guid>();

        private string Configuration { get; }

        public WorkerConfigurator(string configuration)
        {
            Configuration = configuration;
            RegisterBindings();
        }

        private void RegisterBindings()
        {
            var running = Assembly.GetExecutingAssembly();
            var calling = Assembly.GetCallingAssembly();

            RegisterTypes(running);
            if (running != calling)
                RegisterTypes(calling);
        }

        private void RegisterTypes(Assembly assembly)
        {
            Guid GetIdentifier(string str)
            {
                Guid.TryParse(str, out var result);
                return result;
            }

            var worker = typeof(IWorker);
            var results = from clazz in assembly.GetTypes().AsParallel()
                where worker.IsAssignableFrom(clazz)
                let workerType = clazz.GetCustomAttribute<WorkerType>()
                where workerType?.Identifier != Guid.Empty
                select new KeyValuePair<Guid, Type>(workerType.Identifier, clazz);

            bindings.Register(results);
        }

        public T Create<T>(Action<T> action = null) where T : IWorker
        {
            if (!bindings.GetOrNothing(typeof(T), out var group))
            {
                throw new ArgumentException($"Unable to find group identifier for {typeof(T)}");
            }

            var worker = DefaultWorker.NewInstance<T>(group, Guid.NewGuid());
            action?.Invoke(worker);
            return worker;
        }

        public IWorker Create(Type type, Action<IWorker> action = null)
        {
            if (!bindings.GetOrNothing(type, out var group))
            {
                throw new ArgumentException($"Unable to find group identifier for {type}");
            }

            var worker = DefaultWorker.NewInstance(group, Guid.NewGuid(), type);
            action?.Invoke(worker);
            return worker;
        }

        public bool Load(IDictionary<Guid, IWorker> workers)
        {
            try
            {
                using (var stream = new FileStream(Configuration, FileMode.Open, FileAccess.Read))
                {
                    var reader = new BinaryReader(stream);
                    var count = reader.ReadInt32();

                    for (var i = 0; i < count; i++)
                    {
                        var group = reader.ReadGuid();
                        var identifier = reader.ReadGuid();

                        if (!registry.TryGet(group, out var clazz))
                        {
                            continue;
                        }

                        var worker = DefaultWorker.NewInstance(@group, identifier, clazz);

                        var chunksLength = reader.ReadInt32();
                        var chunksBuffer = reader.ReadBytes(chunksLength);

                        try
                        {
                            var buffer = new MemoryStream(chunksBuffer);
                            using (var bufferReader = new BinaryReader(buffer))
                            {
                                worker.LoadExternal(bufferReader);
                            }
                        }
                        catch (Exception ex)
                        {
                            // ToDo: Propagate Exception
                            continue;
                        }

                        workers[worker.Label] = worker;
                    }
                }

                return true;
            }
            catch (FileNotFoundException ex)
            {
                // ToDo: Propagate FileNotFoundException 
            }
            catch (IOException ex)
            {
                // ToDo: Propagate IOException
            }

            return false;
        }

        public bool Save(IDictionary<Guid, IWorker> workers)
        {
            try
            {
                //BUG: using in try catch bad? while catch might not dispose resources?
                using (var stream = new FileStream(Configuration, FileMode.Create, FileAccess.Write))
                {
                    var writer = new BinaryWriter(stream);
                    writer.Write(workers.Count);

                    foreach (var worker in workers.Values)
                    {
                        var clazz = worker.GetType();

                        if (!registry.TryGet(clazz, out var group))
                        {
                            continue;
                        }

                        if (group != worker.Group)
                        {
                            // how did this happen different group ids 
                            continue;
                        }

                        writer.Write(worker.Group);
                        writer.Write(worker.Label);

                        byte[] chunks;
                        try
                        {
                            var buffer = new MemoryStream();
                            using (var bufferWriter = new BinaryWriter(buffer))
                            {
                                worker.SaveExternal(bufferWriter);
                                chunks = buffer.ToArray();
                            }
                        }
                        catch (Exception ex)
                        {
                            // ToDo: Propagate Exception
                            chunks = new byte[0];
                        }

                        writer.Write(chunks.Length);
                        writer.Write(chunks);
                    }
                }

                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                // ToDo: Propagate UnauthorizedAccessException
            }

            return false;
        }
    }
}
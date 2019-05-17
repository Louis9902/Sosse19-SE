using System;
using System.Collections.Generic;
using System.IO;
using Backupper.Common.Extensions;

namespace Backupper.Common
{
    public class WorkerConfiguration
    {
        private readonly WorkerRegistry registry;

        private string Configuration { get; }

        public WorkerConfiguration(string configuration, WorkerRegistry regs)
        {
            Configuration = configuration;
            registry = regs;
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

                        var worker = DefaultWorker.NewInstance(clazz, group, identifier);

                        var chunksLength = reader.ReadInt32();
                        var chunksBuffer = reader.ReadBytes(chunksLength);

                        try
                        {
                            using (var buffer = new MemoryStream(chunksBuffer))
                            {
                                worker.LoadData(buffer);
                            }
                        }
                        catch (Exception ex)
                        {
                            // ToDo: Propagate Exception
                            continue;
                        }

                        workers[worker.Identifier] = worker;
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
                        writer.Write(worker.Identifier);

                        byte[] chunks;
                        try
                        {
                            using (var buffer = new MemoryStream())
                            {
                                worker.SaveData(buffer);
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
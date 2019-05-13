using System;
using System.Collections.Generic;
using System.IO;
using static Backupper.Core.Entrypoint;

namespace Backupper.Core
{
    public class WorkerManager
    {
        private readonly Dictionary<long, Type> register = new Dictionary<long, Type>();
        private readonly Dictionary<Guid, Worker> workers = new Dictionary<Guid, Worker>();

        public WorkerManager()
        {
            WorkerTypeRegister.Register(register);
        }

        public bool StartWorkers()
        {
            return false;
        }

        public bool AbortWorkers()
        {
            return false;
        }

        private void ReadConfig(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                var amount = reader.ReadInt32();
                for (var i = 0; i < amount; i++)
                {
                    var serial = reader.ReadInt64();
                    
                    var ident = reader.ReadGuid();
                    var chunk = reader.ReadInt32();
                    var buffer = reader.ReadBytes(chunk);

                    if (!register.TryGetValue(serial, out var clazz))
                    {
                        // TODO: fix me at some point
                    }

                    //var stream = new MemoryStream(buffer, false);

                    if (!register.TryGetValue(serial, out var kind))
                    {
                        LogException($"File {FileConfig} contains corrupt data, please fix!");
                        continue;
                    }

                    var worker = Worker.NewInstance(kind, this, ident);
                    try
                    {
                        worker.Start(stream);
                    }
                    catch (Exception ex)
                    {
                        LogException($"An error occured while constructing worker instance {ident}: {ex.Message}");
                    }

                    workers.Add(ident, worker);
                }
            }
        }

        private void WriteConfig()
        {
            using (var writer = new BinaryWriter(new FileStream(FileConfig, FileMode.CreateNew)))
            {
                writer.Write(workers.Count);
            }
        }
    }

    public static class StreamCompanion
    {
        public static Guid ReadGuid(this BinaryReader reader)
        {
            return new Guid(reader.ReadBytes(16));
        }

        public static void WriteGuid(this BinaryWriter writer, Guid guid)
        {
            writer.Write(guid.ToByteArray());
        }
    }
}
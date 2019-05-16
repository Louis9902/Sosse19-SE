using System;
using System.Collections.Generic;
using System.IO;
using Backupper.Core.Tasks;
using static Backupper.Core.Entrypoint;

namespace Backupper.Core
{
    public class WorkerManager
    {
        private readonly Dictionary<byte, Type> register = new Dictionary<byte, Type>();
        private readonly Dictionary<Guid, Worker> workers = new Dictionary<Guid, Worker>();
        private readonly WorkerGroups groups = new WorkerGroups();

        public WorkerManager()
        {
            WorkerGroups.Register(register);
        }

        private static FileStream NewConfigStream(FileMode mode)
        {
            return new FileStream(FileConfig, mode);
        }

        public bool StartWorkers()
        {
            using (var stream = NewConfigStream(FileMode.Open))
            {
                ReadConfig(stream);
                return true;
            }

            return false;
        }

        public bool AbortWorkers()
        {
            using (var stream = NewConfigStream(FileMode.CreateNew))
            {
                WriteConfig(stream);
                return true;
            }

            return false;
        }

        private void ReadConfig(Stream stream)
        {
            var reader = new BinaryReader(stream);

            var amount = reader.ReadInt32();
            for (var index = 0; index < amount; index++)
            {
                var group = reader.ReadGuid();
                var ident = reader.ReadGuid();

                if (!groups.FindGroupType(group, out var clazz))
                {
                    LogWarning($"{ident} has corrupt data, please fix!");
                    stream.Seek(reader.ReadInt32(), SeekOrigin.Current);
                    continue;
                }

                var worker = Worker.NewInstance(clazz, group, ident);

                var chunk = reader.ReadInt32();
                var buffer = reader.ReadBytes(chunk);

                try
                {
                    worker.Load(new MemoryStream(buffer));
                }
                catch (Exception ex)
                {
                    // ToDo: Exception Handle
                }

                workers[worker.Group] = worker;
            }
        }

        private void WriteConfig(Stream stream)
        {
            var writer = new BinaryWriter(stream);

            writer.Write(workers.Count);
            foreach (var worker in workers.Values)
            {
                if (!groups.FindGroupType(worker.Group, out var clazz))
                {
                    continue;
                }

                writer.Write(worker.Group);
                writer.Write(worker.Identifier);

                var buffer = new MemoryStream();
                try
                {
                    worker.Save(buffer);
                }
                catch (Exception ex)
                {
                    // ToDo: Exception Handle
                }

                writer.Write(buffer.Length);
                writer.Write(buffer.ToArray());

                workers[worker.Group] = null;
            }
        }
        
    }

    public static class StreamCompanion
    {
        public static Guid ReadGuid(this BinaryReader reader)
        {
            return new Guid(reader.ReadBytes(16));
        }

        public static void Write(this BinaryWriter writer, Guid guid)
        {
            writer.Write(guid.ToByteArray());
        }
    }
    
}
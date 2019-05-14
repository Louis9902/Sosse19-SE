using System;
using System.Collections.Generic;
using System.IO;
using static Backupper.Core.Entrypoint;

namespace Backupper.Core
{
    public class WorkerManager
    {
        private readonly Dictionary<byte, Type> register = new Dictionary<byte, Type>();
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
            using (var stream = new FileStream(FileConfig, FileMode.OpenOrCreate))
            {
                WriteConfig(stream);
            }

            return false;
        }

        private void ReadConfig(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                var amount = reader.ReadInt32();
                for (var i = 0; i < amount; i++)
                {
                    var id = reader.ReadByte();
                    var ident = reader.ReadGuid();
                    var chunk = reader.ReadInt32();

                    if (!register.TryGetValue(id, out var clazz))
                    {
                        LogWarning($"Data corruption for {ident}, please fix!");
                        stream.Seek(chunk, SeekOrigin.Current);
                        continue;
                    }

                    var buffer = new MemoryStream(reader.ReadBytes(chunk), false);
                    var worker = Worker.NewInstance(clazz, this, ident);

                    try
                    {
                        worker.Start(buffer);
                    }
                    catch (Exception ex)
                    {
                        LogException($"An error occured while constructing worker instance {ident}: {ex.Message}");
                    }

                    workers.Add(ident, worker);
                }
            }
        }

        private void WriteConfig(Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(workers.Count);

                foreach (var next in workers)
                {
                    var worker = next.Value;

                    if (!register.TryGetKeyByValue(worker.GetType(), out var id))
                    {
                        DumpData(next);
                        continue;
                    }

                    writer.Write(id);
                    writer.WriteGuid(worker.Identifier);

                    var buffer = new MemoryStream();
                    try
                    {
                        worker.Abort(buffer);
                    }
                    catch (Exception ex)
                    {
                        LogException($"An error occured while constructing worker instance {worker.Identifier}: {ex.Message}");
                    }

                    var array = buffer.ToArray();
                    writer.Write(array.Length);
                    writer.Write(array);
                }
            }
        }

        private void DumpData(KeyValuePair<Guid, Worker> next)
        {
            throw new NotImplementedException();
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

    public static class DictionaryCompanion
    {
        public static bool TryGetKeyByValue<K, V>(this IDictionary<K, V> dictionary, V value, out K result)
        {
            if (dictionary == null) goto ResultNull;

            foreach (var next in dictionary)
            {
                if (!value.Equals(next.Value)) continue;
                result = next.Key;
                return true;
            }

            ResultNull:
            result = default(K);
            return false;
        }
    }
}
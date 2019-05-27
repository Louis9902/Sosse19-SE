using System;
using System.Collections.Generic;
using System.IO;
using WorksKit.Utilities;
using WorksKit.Worker;
using WorksKit.Worker.Group;

namespace WorksKit
{
    public class Workers
    {
        public Workers(string configuration)
        {
            Configuration = configuration;
        }

        private string Configuration { get; }

        /// <summary>
        /// Allows to create a new worker object, this object will get a new guid and the corresponding
        /// group identifier from the <see cref="WorkerGroups"/>
        /// </summary>
        /// <typeparam name="T">The type of the worker to create</typeparam>
        /// <returns>the new worker read toi be instantiated</returns>
        /// <exception cref="ArgumentException">If the type it not registered in the group registry</exception>
        public static T CreateNewWorker<T>() where T : IWorker
        {
            if (WorkerGroups.ObjectBindings.GetOrNothing(typeof(T), out var group))
            {
                return DefaultWorker.Instantiate<T>(group, Guid.NewGuid());
            }

            throw new ArgumentException("missing group for worker");
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
                        var label = reader.ReadGuid();

                        if (!WorkerGroups.ObjectBindings.GetOrNothing(group, out var clazz))
                        {
                            Logger.Warn("Can't find class binding for group id {0}", group);
                            // ToDo: drain further stream
                            continue;
                        }

                        var worker = DefaultWorker.Instantiate(clazz, group, label);

                        var payloadAmount = reader.ReadInt32();
                        var payloadBuffer = reader.ReadBytes(payloadAmount);

                        if (worker is DefaultWorker defaults)
                        {
                            using (var payload = new MemoryStream(payloadBuffer, false))
                            {
                                try
                                {
                                    defaults.LoadPreferences(payload);
                                }
                                catch (Exception ex)
                                {
                                    Logger.Error("An error occurred while loading preferences for {0}", worker);
                                    Logger.Debug("Exception: {0}", ex);
                                }
                            }
                        }
                        else
                        {
                            Logger.Warn("Cannot load preferences for non DefaultWorker");
                            Logger.Debug("Please note Worker must implement DefaultWorker to use preferences");
                        }

                        workers[label] = worker;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Logger.Warn("File {0} not found", Configuration);
                return false;
            }

            return true;
        }

        public bool Save(IDictionary<Guid, IWorker> workers)
        {
            if (!File.Exists(Configuration)) return false;
            try
            {
                using (var stream = new FileStream(Configuration, FileMode.Create, FileAccess.Write))
                {
                    var writer = new BinaryWriter(stream);


                    writer.Write(workers.Count);

                    foreach (var pair in workers)
                    {
                        var worker = pair.Value;

                        if (!WorkerGroups.ObjectBindings.HasObjBinding(worker.GetType()))
                        {
                            Logger.Warn("Can't find class binding for group id {0}@{1}", worker.Group, worker.Label);
                            // ToDo: Implement dumping of preferences
                            continue;
                        }

                        writer.Write(worker.Group);
                        writer.Write(worker.Label);

                        if (worker is DefaultWorker defaults)
                        {
                            byte[] payloadBuffer;

                            using (var payload = new MemoryStream())
                            {
                                defaults.SavePreferences(payload);
                                payloadBuffer = payload.ToArray();
                            }

                            writer.Write(payloadBuffer.Length);
                            writer.Write(payloadBuffer);
                        }
                        else
                        {
                            Logger.Warn("Cannot save preferences for non DefaultWorker");
                            Logger.Debug("Please note Worker must implement DefaultWorker to use preferences");
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                Logger.Warn("Unauthorized access for file {0}", Configuration);
                return false;
            }

            return true;
        }
    }
}
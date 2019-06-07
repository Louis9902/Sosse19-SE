using System;
using System.Collections.Generic;
using System.IO;
using TinyTasksKit.Utilities;
using TinyTasksKit.Worker;
using TinyTasksKit.Worker.Group;

namespace TinyTasksKit
{
    /// <summary>
    /// Represents an adapter which allows the serialisation and deserialization of IWorker objects to a specified file.
    /// This also allows the creation of new IWorker objects. 
    /// </summary>
    public class Workers
    {
        private readonly string configuration;
        
        public Workers(string path)
        {
            configuration = path;
        }

        /// <summary>
        /// Creates a new object of the specified type.
        /// This is important to use instead of the new operator, because this will invoke the required parameters
        /// for the DefaultWorker.
        /// </summary>
        /// <typeparam name="T">The type of IWorker</typeparam>
        /// <returns>The new IWorker object</returns>
        /// <exception cref="ArgumentException">If the specified type is not registered in the WorkerGroups</exception>
        public static T CreateWorker<T>() where T : IWorker
        {
            if (WorkerGroups.ObjectBindings.GetOrNothing(typeof(T), out var group))
            {
                return DefaultWorker.Instantiate<T>(group, Guid.NewGuid());
            }

            throw new ArgumentException("missing group for worker");
        }

        /// <summary>
        /// Loads the IWorker objects from the configuration into the IDictionary.
        /// </summary>
        /// <param name="workers">The IDictionary to store the workers</param>
        /// <returns>true if the operation was successful otherwise false</returns>
        public bool Load(IDictionary<Guid, IWorker> workers)
        {
            if (!File.Exists(configuration)) return false;
            try
            {
                using (var stream = new FileStream(configuration, FileMode.Open, FileAccess.Read))
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
                                    Logger.Error("An error occurred while loading preferences for {0}",
                                        worker.GetType());
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
            catch (IOException)
            {
                Logger.Error("An unexpected io exception occurred while loading from file {0}", configuration);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Saves the IWorker objects from the IDictionary into the configuration.
        /// </summary>
        /// <param name="workers">The IDictionary to read the workers from</param>
        /// <returns>true if the operation was successful otherwise false</returns>
        public bool Save(IDictionary<Guid, IWorker> workers)
        {
            try
            {
                using (var stream = new FileStream(configuration, FileMode.Create, FileAccess.Write))
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
                Logger.Error("An unauthorized access exception occurred while saving to file {0}", configuration);
                return false;
            }

            return true;
        }
    }
}
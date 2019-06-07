using System;
using TinyTasksKit.Worker.Preferences;

namespace TinyTasksKit.Worker
{
    /// <summary>
    /// Represents a simple worker object.
    /// </summary>
    public interface IWorker
    {
        /// <summary>
        /// The Guid of the Type. This is specific for every implementation.
        /// Required for instantiation and serialisation.
        /// </summary>
        /// <seealso cref="TinyTasksKit.Worker.Group.WorkerGroups"/>
        Guid Group { get; }

        /// <summary>
        /// The Guid of this worker. This is unique for every instance of a implementation.
        /// Required for identifying the object.
        /// </summary>
        Guid Label { get; }

        /// <summary>
        /// A collection of all preferences associated with this IWorker implementation.
        /// This is also serialised and stored in the configuration.
        /// </summary>
        /// <seealso cref="TinyTasksKit.Worker.Preferences.PreferenceSet"/> 
        PreferenceSet Preferences { get; }

        /// <summary>
        /// Will start the operations of this worker.
        /// </summary>
        void StartWorker();

        /// <summary>
        /// Will abort the operations of this worker.
        /// </summary>
        void AbortWorker();
    }
}
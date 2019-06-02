using System;
using TinyTasksKit.Worker.Preferences;

namespace TinyTasksKit.Worker
{
    public interface IWorker
    {
        Guid Group { get; }
        Guid Label { get; }

        PreferenceSet Preferences { get; }

        void StartWorker();

        void AbortWorker();
    }
}
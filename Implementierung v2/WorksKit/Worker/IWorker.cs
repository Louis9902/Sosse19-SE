using System;
using WorksKit.Worker.Preferences;

namespace WorksKit.Worker
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
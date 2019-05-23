using System;

namespace WorksKit.Worker
{
    public interface IWorker
    {
        Guid Group { get; }
        Guid Label { get; }
    }
}
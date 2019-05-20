using System;
using System.IO;

namespace Backupper.Common
{
    public interface IWorker : IExternalizable
    {
        Guid Group { get; }
        Guid Label { get; }

        Properties Properties { get; }

        void Start();

        void Abort();
    }
}
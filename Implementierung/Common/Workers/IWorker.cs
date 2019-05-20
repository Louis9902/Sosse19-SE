using System;
using System.IO;
using Backupper.Common.Workers.Properties;

namespace Backupper.Common
{
    public interface IWorker
    {
        Guid Group { get; }
        Guid Label { get; }

        PropertyMap Properties { get; }

        void StartWorker();
        
        void AbortWorker();
    }
}
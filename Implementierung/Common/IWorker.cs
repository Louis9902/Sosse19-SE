using System;
using System.IO;

namespace Backupper.Common
{
    public interface IWorker
    {
        Guid Group { get; }
        Guid Identifier { get; }

        void LoadData(Stream stream);
        
        void SaveData(Stream stream);

        void Start();

        void Abort();

    }
}
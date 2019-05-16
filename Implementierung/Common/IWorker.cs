using System;
using System.IO;

namespace Backupper.Common
{
    public interface IWorker
    {
        Guid Group { get; }
        Guid Identifier { get; }

        bool LoadData(Stream stream);
        
        bool SaveData(Stream stream);

        void Start();

        void Abort();

    }
}
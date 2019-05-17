using System;
using System.IO;

namespace Backupper.Common.Workers
{
    [WorkerType("45ba5697-ec31-43b1-8a39-c4b390d9370a")]
    public class BackupWorker : DefaultWorker
    {
        
        public string Source { get; set; }
        public string Target { get; set; }
        
        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Abort()
        {
            throw new NotImplementedException();
        }

        public override void LoadData(Stream stream)
        {
            var reader = new BinaryReader(stream);
            Source = reader.ReadString();
            Target = reader.ReadString();
        }

        public override void SaveData(Stream stream)
        {
            var writer = new BinaryWriter(stream);
            writer.Write(Source);
            writer.Write(Target);
        }
    }
}
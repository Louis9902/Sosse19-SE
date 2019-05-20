using System;
using System.IO;

namespace Backupper.Common.Workers
{
    [WorkerType("45ba5697-ec31-43b1-8a39-c4b390d9370a")]
    public class BackupWorker : DefaultWorker
    {
        
//        public string Source
//        {
//            get => (string) Properties["source"];
//            set => Properties["source"] = value;
//        }
        
        
        public string Target { get; set; }
        
        public override void StartWorker()
        {
            throw new NotImplementedException();
        }

        public override void AbortWorker()
        {
            throw new NotImplementedException();
        }
    }
}
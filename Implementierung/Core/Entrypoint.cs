using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backupper.Common;
using Backupper.Common.Workers;

namespace Backupper.Core
{
    public class Entrypoint
    {
        public const string FileConfig = "./Config.txt";
        private static WorkerManager manager;

        public static void Main(string[] args)
        {
            var regs = new WorkerRegistry();
            var conf = new WorkerConfiguration(FileConfig, regs);
//            Console.WriteLine((string) regs);

            var workers = new Dictionary<Guid, IWorker>();

//            if (!conf.Load(workers))
//            {
//                var worker = DefaultWorker.Create<BackupWorker>(regs, arg =>
//                {
//                    arg.Source = "C://Some/Path/Source";
//                    arg.Target = "C://Some/Path/Target";
//                });
//                workers[worker.Label] = worker;
//                Console.WriteLine("--> ADDED NEW");
//            }

            Console.WriteLine(workers.Count);
            foreach (var pair in workers)
            {
                Console.WriteLine($"{pair.Value.GetType().Name}@{pair.Key}");
            }

            if (!conf.Save(workers))
            {
                Console.WriteLine("Something is broken, please fix!");
            }
        }

        public static void LogException(string line)
        {
            Console.Error.WriteLine(line);
        }

        public static void LogWarning(string line)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using TinyTasksKit;
using TinyTasksKit.Worker;
using TinyTasksKit.Worker.Group;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var workers = new Workers("./ExampleWorkers.dat");

            var cache = new Dictionary<Guid, IWorker>();

            if (!workers.Load(cache))
            {
                var sync = typeof(SyncWorker);
                
                if (WorkerGroups.ObjectBindings.GetOrNothing(sync, out var group))
                {
                    var worker = DefaultWorker.Instantiate<SyncWorker>(group,Guid.NewGuid());
                    worker.Source = "c://some//more//source";
                    worker.Target = "c://some//more//target";
                    cache[worker.Label] = worker;
                }
                else
                {
                    Console.Error.Write("Missing Registry Keys");
                }
            }
            else
            {
                Console.WriteLine("loaded from file " + cache.Count);
            }

            foreach (var pair in cache)
            {
                Console.WriteLine($"{pair.Key} -> {pair.Value}");
            }

            if (!workers.Save(cache))
            {
                Console.Error.Write("Workers save failed");
            }
        }
    }
}
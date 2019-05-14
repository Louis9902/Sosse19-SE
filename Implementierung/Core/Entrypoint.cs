using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backupper.Core
{
    public class Entrypoint
    {

        public const string FileConfig = "./Config.txt";
        private static WorkerManager manager;

        public static void Main(string[] args)
        {
            manager = new WorkerManager();
            manager.StartWorkers();

            AppDomain.CurrentDomain.ProcessExit += (sender, arg) => manager.AbortWorkers();
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using TinyTasksKit;
using TinyTasksKit.Worker;

namespace TinyTasksService
{
    public class Program
    {
        private static bool? hasConsole;
        private static string configuration;

        public static void Main(string[] args)
        {
            var skipHeadless = false;

            if (args.Length != 0)
            {
                var index = 0;
                do
                {
                    var name = args[index++];
                    switch (name)
                    {
                        case "--debug":
                        {
                            skipHeadless = true;
                            index++;
                            configuration = args[index++];
                            break;
                        }

                        case "--file":
                        {
                            configuration = args[index++];
                            break;
                        }
                    }
                } while (index < args.Length);
            }

            if (!skipHeadless && HasConsole)
            {
                var info = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    FileName = typeof(Program).Assembly.Location,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                try
                {
                    using (var process = Process.Start(info))
                    {
                        process?.WaitForExit();
                    }
                }
                catch (Exception)
                {
                    Logger.Error("Something happened while trying to run headless");
                }

                return;
            }

            if (string.IsNullOrEmpty(configuration))
            {
                Logger.Error("Missing config file");
                return;
            }

            var workers = new Workers(configuration);
            var workersCache = new Dictionary<Guid, IWorker>();

            Logger.Erroring += Console.WriteLine;

            workers.Load(workersCache);

            AppDomain.CurrentDomain.ProcessExit += (sender, arg) =>
            {
                foreach (var worker in workersCache.Values)
                {
                    try
                    {
                        worker.AbortWorker();
                    }
                    catch
                    {
                    }
                }

                workers.Save(workersCache);
            };

            foreach (var worker in workersCache.Values)
            {
                try
                {
                    worker.StartWorker();
                }
                catch
                {
                }
            }
        }

        private static bool HasConsole
        {
            get
            {
                if (hasConsole != null) return hasConsole.Value;
                hasConsole = true;
                try
                {
                    var height = Console.WindowHeight;
                }
                catch
                {
                    hasConsole = false;
                }

                return hasConsole.Value;
            }
        }
    }
}
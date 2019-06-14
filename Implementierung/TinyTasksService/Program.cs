using System;
using System.Collections.Generic;
using System.IO;
using TinyTasksKit;
using TinyTasksKit.Worker;

namespace TinyTasksService
{
    public class Program
    {
        private static string configuration = Workers.GetDefaultConfiguration();
        private static bool headless = true;

        public static void Main(string[] args)
        {
            Logger.Erroring += Console.Error.WriteLine;
            Logger.Warning += Console.Out.WriteLine;

#if DEBUG
            Logger.Debugging += Console.Out.WriteLine;
#endif

            ParseArguments(args);

            if (headless)
            {
                HideConsole(false, Console.Title);
            }

            if (string.IsNullOrEmpty(configuration) && !File.Exists(configuration))
            {
                Logger.Error("No configuration file is specified or file is not found");
                return;
            }

            var manager = new Workers(configuration);
            var workers = new Dictionary<Guid, IWorker>();

            manager.Load(workers);

            foreach (var worker in workers.Values) RunSafe(() => worker.StartWorker());

            AppDomain.CurrentDomain.ProcessExit += (sender, arg) =>
            {
                foreach (var worker in workers.Values) RunSafe(() => worker.AbortWorker());
                manager.Save(workers);
            };

            while (Console.ReadKey().Key != ConsoleKey.Escape) ;
        }

        private static void HideConsole(bool visible, string title)
        {
            var window = User32.FindWindow(null, title);
            if (window == IntPtr.Zero) return;
            User32.ShowWindow(window, !visible ? 0 : 1);
        }

        private static void ParseArguments(IReadOnlyList<string> args)
        {
            if (args.Count < 1) return;

            var length = args.Count;
            var index = 0;

            bool HasMoreArgs(int amount)
            {
                return index + (amount - 1) < length;
            }

            do
            {
                var name = args[index++];
                switch (name)
                {
                    case "--debug":
                    {
                        headless = false;
                        break;
                    }

                    case "-f" when HasMoreArgs(1):
                    case "--file" when HasMoreArgs(1):
                    {
                        configuration = args[index++];
                        break;
                    }

                    default:
                    {
                        Logger.Warn("Unknown argument {0}", name);
                        break;
                    }
                }
            } while (index < args.Count);
        }

        private static void RunSafe(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Logger.Error("An error occured while executing some actions: {0}", ex);
            }
        }
    }
}
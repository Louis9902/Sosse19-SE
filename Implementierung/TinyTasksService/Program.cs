using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using TinyTasksKit;
using TinyTasksKit.Worker;

namespace TinyTasksService
{
    public class Program
    {
        private static bool? hasConsole;

        private static string configuration;
        private static bool headless = true;

        public static void Main(string[] args)
        {
            Logger.Erroring += Console.Error.WriteLine;
            Logger.Warning += Console.Out.WriteLine;
            Console.WriteLine(args);
            Arguments(args);

            if (headless && HasConsole)
            {
                setConsoleWindowVisibility(false, Console.Title);
            }

            if (string.IsNullOrEmpty(configuration) && !File.Exists(configuration))
            {
                Logger.Error("No configuration file is specified or file is not found");
                return;
            }

            var workers = new Workers(configuration);
            var cache = new Dictionary<Guid, IWorker>();

            workers.Load(cache);

            foreach (var worker in cache.Values)
            {
                RunSafe(() => worker.StartWorker());
            }

            AppDomain.CurrentDomain.ProcessExit += (sender, arg) =>
            {
                foreach (var worker in cache.Values)
                {
                    RunSafe(() => worker.AbortWorker());
                }

                workers.Save(cache);
            };

            while (Console.ReadKey().KeyChar != 'q') ;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void setConsoleWindowVisibility(bool visible, string title)
        {
            // below is Brandon's code           
            //Sometimes System.Windows.Forms.Application.ExecutablePath works for the caption depending on the system you are running under.          
            IntPtr hWnd = FindWindow(null, title);

            if (hWnd != IntPtr.Zero)
            {
                if (!visible)
                    //Hide the window                   
                    ShowWindow(hWnd, 0); // 0 = SW_HIDE               
                else
                    //Show window again                   
                    ShowWindow(hWnd, 1); //1 = SW_SHOWNORMA          
            }
        }

        private static void Arguments(IReadOnlyList<string> args)
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
using System;

namespace Backupper.Common
{
    public static class Logger
    {
        public static void Info(string message)
        {
            Console.Out.WriteLine("[INFO]: " + message);
        }

        public static void Warn(string message)
        {
            Console.Out.WriteLine("[WARN]: " + message);
        }

        public static void Error(string message)
        {
            Console.Error.WriteLine("[ERROR]: " + message);
        }
    }
}
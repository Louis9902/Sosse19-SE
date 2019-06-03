using System;
using System.IO;

namespace TinyTasksKit
{
    public static class Logger
    {
        private const string HeaderInfo = "[Info]: ";
        private const string HeaderWarn = "[Warn]: ";
        private const string HeaderDebug = "[Debug]: ";
        private const string HeaderError = "[Error]: ";
        private const string HeaderTrace = "[Trace]: ";

        public static event Action<string> Informing;
        public static event Action<string> Warning;
        public static event Action<string> Debugging;
        public static event Action<string> Erroring;
        public static event Action<string> Tracing;

        public static void Info(string message, params object[] objects)
        {
            Informing?.Invoke(HeaderInfo + String.Format(message, objects));
        }

        public static void Warn(string message, params object[] objects)
        {
            Warning?.Invoke(HeaderWarn + String.Format(message, objects));
        }

        public static void Debug(string message, params object[] objects)
        {
            Debugging?.Invoke(HeaderDebug + String.Format(message, objects));
        }

        public static void Error(string message, params object[] objects)
        {
            Erroring?.Invoke(HeaderError + String.Format(message, objects));
        }

        public static void Trace(string message, params object[] objects)
        {
            Tracing?.Invoke(HeaderTrace + String.Format(message, objects));
        }
    }
}
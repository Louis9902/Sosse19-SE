using System;
using static System.String;

namespace WorksKit
{
    public static class Logger
    {
        private const string HeaderInfo = "[Info]: ";
        private const string HeaderWarn = "[Warn]: ";
        private const string HeaderDebug = "[Debug]: ";
        private const string HeaderError = "[Error]: ";

        public static event Action<string> Informing;
        public static event Action<string> Warning;
        public static event Action<string> Debugging;
        public static event Action<string> Erroring;

        public static void Info(string message, params object[] objects)
        {
            Informing?.Invoke(HeaderInfo + Format(message, objects));
        }

        public static void Warn(string message, params object[] objects)
        {
            Warning?.Invoke(HeaderWarn + Format(message, objects));
        }

        public static void Debug(string message, params object[] objects)
        {
            Debugging?.Invoke(HeaderDebug + Format(message, objects));
        }

        public static void Error(string message, params object[] objects)
        {
            Erroring?.Invoke(HeaderError + Format(message, objects));
        }
    }
}
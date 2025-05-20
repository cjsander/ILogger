using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace logging
{
    public static class AppLogger
    {
        private static readonly object _lock = new();
        private static string _logFilePath = "app.txt";

        public static void SetLogFilePath(string path)
        {
            _logFilePath = path;
        }

        public static void LogInfo(string message,
            [CallerMemberName] string memberName = "") =>
            WriteLog("INFO", message, memberName);

        public static void LogWarning(string message,
            [CallerMemberName] string memberName = "") =>
            WriteLog("WARNING", message, memberName);

        public static void LogError(string message, Exception ex = null,
            [CallerMemberName] string memberName = "") =>
            WriteLog("ERROR", $"{message} {(ex != null ? $"| Exception: {ex}" : "")}", memberName);

        private static void WriteLog(string level, string message, string memberName)
        {
            var logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] ({memberName}) {message}{Environment.NewLine}";

            lock (_lock)
            {
                try
                {
                    File.AppendAllText(_logFilePath, logLine);
                }
                catch
                {
                    // Suppress logging errors
                }
            }
        }
    }
}

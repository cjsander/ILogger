using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Logging
{
    public static class AppLogger
    {
        private static readonly object _lock = new();
        private static string _logDirectory = "logs";
        private static string _logFilePrefix = "log";

        /// <summary>
        /// Sets the directory and file prefix for log files.
        /// Example: SetLogDirectory("C:\\Logs\\MyApp", "webapi-log") will create logs like webapi-log-2025-05-20.txt
        /// </summary>
        public static void SetLogDirectory(string folderPath, string filePrefix = "log")
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                _logDirectory = folderPath;
                _logFilePrefix = filePrefix;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to set log directory: " + ex.Message);
                _logDirectory = ".";
            }
        }

        public static void LogInfo(string message, [CallerMemberName] string memberName = "") =>
            WriteLog("INFO", message, memberName);

        public static void LogWarning(string message, [CallerMemberName] string memberName = "") =>
            WriteLog("WARNING", message, memberName);

        public static void LogError(string message, Exception ex = null, [CallerMemberName] string memberName = "") =>
            WriteLog("ERROR", $"{message} {(ex != null ? $"| Exception: {ex}" : "")}", memberName);

        private static void WriteLog(string level, string message, string memberName)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string fileName = $"{_logFilePrefix}-{date}.txt"; // Enforces .txt
            string fullPath = Path.Combine(_logDirectory, fileName);

            string logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] ({memberName}) {message}{Environment.NewLine}";

            lock (_lock)
            {
                try
                {
                    File.AppendAllText(fullPath, logLine);
                }
                catch
                {
                    // Optional: fallback to console
                    Console.WriteLine("Logging failed: " + logLine);
                }
            }
        }
    }
}

//hard code for my use only
AppLogger.SetLogDirectory(@"C:\Logs\MyApp", "webapi-log");
AppLogger.LogInfo("App started");
AppLogger.LogError("Something went wrong", new Exception("Oops"));

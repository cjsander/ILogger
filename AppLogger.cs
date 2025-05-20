using Serilog;
using System;
using System.Runtime.CompilerServices;

public static class AppLogger
{
    public static void Info(string message, [CallerMemberName] string memberName = "")
    {
        Log.Information("[{Method}] {Message}", memberName, message);
    }

    public static void Debug(string message, [CallerMemberName] string memberName = "")
    {
        Log.Debug("[{Method}] {Message}", memberName, message);
    }

    public static void Warning(string message, [CallerMemberName] string memberName = "")
    {
        Log.Warning("[{Method}] {Message}", memberName, message);
    }

    public static void Error(string message, [CallerMemberName] string memberName = "")
    {
        Log.Error("[{Method}] {Message}", memberName, message);
    }

    public static void Error(Exception ex, string message = "", [CallerMemberName] string memberName = "")
    {
        Log.Error(ex, "[{Method}] {Message}", memberName, message);
    }

    public static void Fatal(string message, [CallerMemberName] string memberName = "")
    {
        Log.Fatal("[{Method}] {Message}", memberName, message);
    }

    public static void Fatal(Exception ex, string message = "", [CallerMemberName] string memberName = "")
    {
        Log.Fatal(ex, "[{Method}] {Message}", memberName, message);
    }
}

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

public static class LoggingExtensions
{
    public static IDisposable LogMethodScope<T>(
        this ILogger<T> logger,
        string? extraInfo = null,
        [CallerMemberName] string methodName = "")
    {
        var stopwatch = Stopwatch.StartNew();
        logger.LogInformation("➡️ Entering {Method}{Extra}", methodName, FormatExtra(extraInfo));

        return new ScopeLogger<T>(logger, methodName, stopwatch);
    }

    private static string FormatExtra(string? info)
    {
        return string.IsNullOrWhiteSpace(info) ? "" : $" ({info})";
    }

    private class ScopeLogger<T> : IDisposable
    {
        private readonly ILogger<T> _logger;
        private readonly string _methodName;
        private readonly Stopwatch _stopwatch;

        public ScopeLogger(ILogger<T> logger, string methodName, Stopwatch stopwatch)
        {
            _logger = logger;
            _methodName = methodName;
            _stopwatch = stopwatch;
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _logger.LogInformation("⬅️ Exiting {Method} after {ElapsedMs} ms", _methodName, _stopwatch.ElapsedMilliseconds);
        }
    }
}

public class MyService
{
    private readonly ILogger<MyService> _logger;

    public MyService(ILogger<MyService> logger)
    {
        _logger = logger;
    }

    public void ProcessData(string user)
    {
        using (_logger.LogMethodScope(extraInfo: $"user={user}"))
        {
            // Simulate work
            Thread.Sleep(200);
        }
    }
}

//for void methods:
_logger.ExecuteLogged(() =>
{
    // Your method logic
    if (DateTime.Now.Second % 2 == 0)
        throw new InvalidOperationException("Test error!");
});

//for return methods:
var result = _logger.ExecuteLogged(() =>
{
    // Logic here...
    return "Hello";
});

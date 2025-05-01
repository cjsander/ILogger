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

        return new ScopedLogger<T>(logger, methodName, stopwatch);
    }

    private static string FormatExtra(string? info)
    {
        return string.IsNullOrWhiteSpace(info) ? "" : $" ({info})";
    }

    private class ScopedLogger<T> : IDisposable
    {
        private readonly ILogger<T> _logger;
        private readonly string _methodName;
        private readonly Stopwatch _stopwatch;
        private Exception? _exception;

        public ScopedLogger(ILogger<T> logger, string methodName, Stopwatch stopwatch)
        {
            _logger = logger;
            _methodName = methodName;
            _stopwatch = stopwatch;
        }

        public void Dispose()
        {
            _stopwatch.Stop();

            if (_exception != null)
            {
                _logger.LogError(_exception, "❌ Exception in {Method} after {ElapsedMs} ms", _methodName, _stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogInformation("⬅️ Exiting {Method} after {ElapsedMs} ms", _methodName, _stopwatch.ElapsedMilliseconds);
            }
        }

        public void SetException(Exception ex)
        {
            _exception = ex;
        }
    }

    public static void ExecuteLogged<T>(
        this ILogger<T> logger,
        Action action,
        string? extraInfo = null,
        [CallerMemberName]


_logger.ExecuteLogged(() =>
{
    // Your method logic
    if (DateTime.Now.Second % 2 == 0)
        throw new InvalidOperationException("Test error!");
});

var result = _logger.ExecuteLogged(() =>
{
    // Logic here...
    return "Hello";
});

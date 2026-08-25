using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace DistriHub.Logging
{
    public class FileLoggerOptions
    {
        public string? LogDirectory { get; set; }
        public string FileNamePrefix { get; set; } = "distrihub";
    }

    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly FileLoggerOptions _options;
        private readonly object _lock = new object();

        public FileLoggerProvider(FileLoggerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(_options.LogDirectory))
                _options.LogDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");

            Directory.CreateDirectory(_options.LogDirectory);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName, _options, _lock);
        }

        public void Dispose()
        {
            // nothing to dispose
        }
    }

    internal class FileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly FileLoggerOptions _options;
        private readonly object _lock;

        public FileLogger(string categoryName, FileLoggerOptions options, object @lock)
        {
            _categoryName = categoryName;
            _options = options;
            _lock = @lock;
        }

        public IDisposable BeginScope<TState>(TState state) => null!;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Warning;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = formatter(state, exception);
            var logRecord = BuildLogRecord(logLevel, eventId, message, exception);

            var filePath = GetLogFilePath();
            try
            {
                lock (_lock)
                {
                    File.AppendAllText(filePath, logRecord + Environment.NewLine);
                }
            }
            catch
            {
                // Swallow logging failures to avoid crashing the app
            }
        }

        private string GetLogFilePath()
        {
            var fileName = $"{_options.FileNamePrefix}-{DateTime.UtcNow:yyyy-MM-dd}.txt";
            return Path.Combine(_options.LogDirectory!, fileName);
        }

        private string BuildLogRecord(LogLevel level, EventId eventId, string message, Exception? ex)
        {
            var ts = DateTime.UtcNow.ToString("o");
            var header = $"[{ts}] {level} {eventId.Id} {_categoryName}:";
            if (ex == null)
                return header + " " + message;

            return header + " " + message + "\n" + ex + "\n";
        }
    }

    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, Action<FileLoggerOptions>? configure = null)
        {
            //this new
            var options = new FileLoggerOptions();
            configure?.Invoke(options);
            builder.AddProvider(new FileLoggerProvider(options));
            return builder;
        }
    }
}

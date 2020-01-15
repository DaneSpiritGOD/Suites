using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;
using static Suites.Extensions.Logging.Memory.Resource;

namespace Microsoft.Extensions.Logging.Memory
{
    internal class MemoryLogger : ILogger
    {
        private readonly string _name;
        private readonly IMemoryLoggerWriter _storage;

        public MemoryLogger(string name, IMemoryLoggerWriter storage)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public IDisposable BeginScope<TState>(TState state)
            => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel)
            => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                if (formatter == null)
                {
                    throw new ArgumentNullException("formatter");
                }
                var text = formatter(state, exception);
                if (!string.IsNullOrEmpty(text) || exception != null)
                {
                    _storage.Append(CreateMessage(logLevel, _name, eventId.Id, text, exception));
                }
            }
        }

        private static string GetLogLevelString(LogLevel logLevel)
        => logLevel switch
        {
            LogLevel.Trace => TraceString,
            LogLevel.Debug => DebugString,
            LogLevel.Information => InformationString,
            LogLevel.Warning => WarningString,
            LogLevel.Error => ErrorString,
            LogLevel.Critical => CriticalString,
            _ => throw new ArgumentOutOfRangeException("logLevel")
        };

        private static readonly string _messagePadding = "    ";
        private StringBuilder _logBuilder;
        private string CreateMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
        {
            var logBuilder = GetStringBuilder();

            logBuilder.AppendFormat("{0} {1} | {2}[{3}]", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), GetLogLevelString(logLevel), logName, eventId);
            if (!string.IsNullOrEmpty(message))
            {
                logBuilder.AppendLine()
                    .Append(_messagePadding)
                    .Append(message);
            }
            if (exception != null)
            {
                logBuilder.AppendLine()
                    .AppendLine(exception.ToString());
            }
            return logBuilder.ToString();

            StringBuilder GetStringBuilder()
            {
                if (_logBuilder == default)
                {
                    _logBuilder = new StringBuilder();
                }
                else
                {
                    _logBuilder.Clear();
                    if (_logBuilder.Capacity > 1024)
                    {
                        _logBuilder.Capacity = 1024;
                    }
                }
                return _logBuilder;
            }
        }
    }

    public interface IMemoryLoggerStorage
    {
        ReadOnlyObservableCollection<string> LoggerCollection { get; }
    }

    public interface IMemoryLoggerWriter
    {
        void Append(string message);
    }

    internal class MemoryLoggerStorage : IMemoryLoggerStorage, IMemoryLoggerWriter
    {
        private readonly int _max;
        private readonly ObservableCollection<string> _core;
        private readonly object _root;

        private ReadOnlyObservableCollection<string> _loggerCollection;
        ReadOnlyObservableCollection<string> IMemoryLoggerStorage.LoggerCollection
            => _loggerCollection ??= new ReadOnlyObservableCollection<string>(_core);

        public MemoryLoggerStorage(int max)
        {
            _max = max <= 0 ? int.MaxValue : max;
            _core = new ObservableCollection<string>();
            _root = ((ICollection)_core).SyncRoot;
        }

        public void Append(string message)
        {
            lock (_root)
            {
                if (_core.Count >= _max)
                {
                    _core.RemoveAt(0);
                }
                _core.Add(message);
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Pending>")]
    public class MemoryLoggerProvider : ILoggerProvider, IDisposable
    {
        private readonly IMemoryLoggerWriter _storage;
        public MemoryLoggerProvider(IMemoryLoggerWriter storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public ILogger CreateLogger(string name) => new MemoryLogger(name, _storage);

        public void Dispose()
        {
        }
    }
}

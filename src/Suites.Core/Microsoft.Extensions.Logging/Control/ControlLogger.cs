using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.Control
{
    public class ControlLogger : ILogger
    {
        private class NoopDisposable : IDisposable
        {
            public static NoopDisposable Instance = new NoopDisposable();

            public void Dispose()
            {
            }
        }

        private readonly Func<string, LogLevel, bool> _filter;
        private readonly IControl _control;
        private readonly string _name;

        public ControlLogger(string name, IControl control)
            : this(name, null, control)
        {
        }

        public ControlLogger(string name, Func<string, LogLevel, bool> filter, IControl control)
        {
            _control = NamedNullException.Assert(control, nameof(control));
            _name = (string.IsNullOrEmpty(name) ? "ControlLogger" : name);
            _filter = filter;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel != LogLevel.None)
            {
                if (_filter != null)
                {
                    return _filter(_name, logLevel);
                }
                return true;
            }
            return false;
        }

        private static readonly IReadOnlyDictionary<LogLevel, string> LogLevelInChinese = new Dictionary<LogLevel, string>
        {
            [LogLevel.Critical] = "致命",
            [LogLevel.Debug] = "调试",
            [LogLevel.Error] = "错误",
            [LogLevel.Information] = "信息",
            [LogLevel.None] = "(无)",
            [LogLevel.Trace] = "跟踪",
            [LogLevel.Warning] = "警告"
        };
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                if (formatter == null)
                {
                    throw new ArgumentNullException("formatter");
                }
                string text = formatter(state, exception);
                if (!string.IsNullOrEmpty(text))
                {
                    text = $"{DateTime.Now.ToLongTimeString()} {LogLevelInChinese[logLevel]}: {text}{Environment.NewLine}";
                    if (exception != null)
                    {
                        text = text + Environment.NewLine + Environment.NewLine + exception.ToString();
                    }
                    _control.Append(text);
                }
            }
        }
    }
}

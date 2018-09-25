using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suites.Core.Microsoft.Extensions.Logging.Control
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
                    text = $"{logLevel}: {text}";
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

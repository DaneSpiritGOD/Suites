using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.Control
{
    public class ControlLoggerProvider : ILoggerProvider, IDisposable
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly IControl _control;

        public ControlLoggerProvider(IControl control)
        {
            _filter = null;
            _control = NamedNullException.Assert(control, nameof(control));
        }

        public ControlLoggerProvider(Func<string, LogLevel, bool> filter)
        {
            _filter = filter;
        }

        public ILogger CreateLogger(string name)
        {
            return new ControlLogger(name, _filter, _control);
        }

        public void Dispose()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Microsoft.Extensions.Logging.Control
{
    public class TimedRefreshControl : IControl
    {
        private readonly Timer _timer;
        private readonly StringBuilder _sb;
        private readonly object _synObj;
        private readonly IControl _control;

        public TimedRefreshControl(IControl control, int timeInMillisecond)
        {
            var time = Math.Max(20, timeInMillisecond);
            _timer = new Timer(timerCallback, new object(), 5000, time);
            _sb = new StringBuilder();
            _synObj = new object();
            _control = NamedNullException.Assert(control, nameof(control));
        }

        private void timerCallback(object state)
        {
            lock (_synObj)
            {
                if (_sb.Length != 0)
                {
                    _control.Append(_sb.ToString());
                    _sb.Clear();
                }
            }
        }

        public void Append(string text)
        {
            lock (_synObj)
            {
                _sb.Append(text);
            }
        }

        public void Clear()
        {
            lock (_synObj)
            {
                _sb.Clear();
                _control.Clear();
            }
        }
    }
}

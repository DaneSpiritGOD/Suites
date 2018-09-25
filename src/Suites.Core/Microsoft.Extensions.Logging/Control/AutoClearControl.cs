using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.Control
{
    public class AutoClearControl : IControl
    {
        private readonly Condition _condition;
        private readonly int _maxCount;
        private readonly IControl _control;

        public AutoClearControl(IControl control, int maxCount)
        {
            _condition = maxCount >= 1 ? new Condition(maxCount) : default;
            _maxCount = maxCount;
            _control = NamedNullException.Assert(control, nameof(control));
        }

        public void Append(string text)
        {
            if (_condition != default && _condition.Trigger())
            {
                _control.Clear();
                _condition.Restore();
            }

            _control.Append(text);
        }

        public void Clear()
        {
            _control.Clear();
        }

        private class Condition
        {
            private int _curCount;
            private readonly int _maxCount;
            public Condition(int maxCount)
            {
                if (maxCount <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(maxCount) + "must be greater than 0.");
                }

                _maxCount = maxCount;
                _curCount = 0;
            }

            public bool Trigger()
            {
                return _curCount++ >= _maxCount;
            }

            public void Restore()
            {
                _curCount = 0;
            }
        }
    }
}

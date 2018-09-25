using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Microsoft.Extensions.Logging.Control
{
    public class TextBoxControl : IControl
    {
        private readonly TextBox _textBox;

        public TextBoxControl(TextBox textBox)
        {
            _textBox = NamedNullException.Assert(textBox, nameof(textBox));
        }

        public void Append(string text)
        {
            _textBox.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action<string>(msg =>
            {
                _textBox.AppendText(msg);
                _textBox.ScrollToEnd();
            }), text);
        }

        public void Clear()
        {
            _textBox.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => _textBox.Clear()));
        }
    }
}

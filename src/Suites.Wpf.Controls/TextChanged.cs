using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Suites.Wpf.Controls
{
    public delegate void TextChangedEventHandler2(object sender, TextChangedEventArgs2 e);

    public class TextChangedEventArgs2 : RoutedEventArgs
    {
        public string NewText { get; }
        public TextChangedEventArgs2(string newText)
        {
            NewText = newText;
        }
    }
}

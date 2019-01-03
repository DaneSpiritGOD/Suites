using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Suites.Wpf.Controls
{
    public static class TextBoxHelper
    {
        public static string GetNullText(DependencyObject obj)
            => (string)obj.GetValue(NullTextProperty);

        public static void SetNullText(DependencyObject obj, string value)
            => obj.SetValue(NullTextProperty, value);

        public static readonly DependencyProperty NullTextProperty =
            DependencyProperty.RegisterAttached("NullText", typeof(string), typeof(TextBoxHelper), new UIPropertyMetadata(null));
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Suites.Wpf.Core
{
    public static class WindowAttacher
    {
        //这个有个要求：Window必须以ShowDialog显示
        #region DialogResult
        public static readonly DependencyProperty DialogResultProperty =
                   DependencyProperty.RegisterAttached(
                    "DialogResult",
                    typeof(bool?),
                    typeof(WindowAttacher),
                    new PropertyMetadata(DialogResultChanged));

        private static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            //var window = d as Window;
            //if (window != null)
            //    window.DialogResult = e.NewValue as bool?;
            if (d is Window window)
                window.DialogResult = e.NewValue as bool?;
        }

        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }

        //public static bool? GetDialogResult(DependencyObject obj)
        //{
        //    return (bool?)obj.GetValue(DialogResultProperty);
        //}
        #endregion DialogResult

        #region Close
        public static void SetClose(DependencyObject target, bool value)
        {
            target.SetValue(CloseProperty, value);
        }
        public static readonly DependencyProperty CloseProperty =
            DependencyProperty.RegisterAttached(
            "Close",
            typeof(bool),
            typeof(WindowAttacher),
            new UIPropertyMetadata(false, OnClose));

        private static void OnClose(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool && ((bool)e.NewValue))
            {
                Window window = GetWindow(sender);
                if (window != null)
                    window.Close();
            }
        }
        private static Window GetWindow(DependencyObject sender)
        {
            Window window = null;
            if (sender is Window)
                window = (Window)sender;
            if (window == null)
                window = Window.GetWindow(sender);
            return window;
        }
        #endregion Close
    }
}

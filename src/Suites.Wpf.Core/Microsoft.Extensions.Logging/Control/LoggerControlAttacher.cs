using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Extensions.Logging.Control
{
    public static class LoggerControlAttacher
    {
        public static readonly DependencyProperty WireProperty =
            DependencyProperty.RegisterAttached(
                "Wire",
                typeof(bool),
                typeof(LoggerControlAttacher),
                new PropertyMetadata(false, WireChanged));

        public static bool GetWire(DependencyObject obj)
        {
            return (bool)obj.GetValue(WireProperty);
        }

        public static void SetWire(DependencyObject obj, bool value)
        {
            obj.SetValue(WireProperty, value);
        }

        private static void WireChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var services = HostBuilderExt.DefaultServiceProvider;
            if (services != default)
            {
                var maxCount = GetAutoClearWhenMeetCount(d);
                var time = GetRefreshInterval(d);

                var tb = NamedNullException.Assert(d as TextBox, nameof(TextBox));
                var ctrl = new TimedRefreshControl(new AutoClearControl(new TextBoxControl(tb), maxCount), time);
                var loggerProvider = new ControlLoggerProvider(ctrl);
                services.GetRequiredService<ILoggerFactory>().AddProvider(loggerProvider);
            }
        }

        public static readonly DependencyProperty RefreshIntervalProperty =
            DependencyProperty.RegisterAttached(
                "RefreshInterval",
                typeof(int),
                typeof(LoggerControlAttacher),
                new PropertyMetadata(2000));

        public static int GetRefreshInterval(DependencyObject obj)
        {
            return (int)obj.GetValue(RefreshIntervalProperty);
        }

        public static void SetRefreshInterval(DependencyObject obj, int value)
        {
            obj.SetValue(RefreshIntervalProperty, value);
        }

        public static readonly DependencyProperty AutoClearWhenMeetCountProperty =
            DependencyProperty.RegisterAttached(
                "AutoClearWhenMeetCount",
                typeof(int),
                typeof(LoggerControlAttacher),
                new PropertyMetadata(1000));

        public static int GetAutoClearWhenMeetCount(DependencyObject obj)
        {
            return (int)obj.GetValue(AutoClearWhenMeetCountProperty);
        }

        public static void SetAutoClearWhenMeetCount(DependencyObject obj, int value)
        {
            obj.SetValue(AutoClearWhenMeetCountProperty, value);
        }
    }
}

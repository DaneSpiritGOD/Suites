using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Logging;

namespace Suites.Wpf.Core
{
    public static class WpfUtils
    {
        public static void CriticalPopup(this Exception ex, ILogger logger = default)
        {
            const string CRITICAL = "严重错误";

            logger?.LogCritical(ex, CRITICAL);
            MessageBox.Show(ex.ToString(), CRITICAL, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

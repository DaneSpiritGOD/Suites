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
        public static void CriticalPopup(this Exception ex, ILogger logger)
        {
            logger.LogCritical(ex,"严重错误。");
            MessageBox.Show(ex.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

using Prism.Interactivity;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Suites.Prism.Wpf.Fx.Interactivity
{
    public class PopupNewWindowAction : PopupWindowAction
    {
        protected override Window GetWindow(INotification notification)
        {
            var window = Activator.CreateInstance(WindowContentType) as Window;
            window.DataContext = notification;
            window.Title = notification.Title;
            if (base.AssociatedObject != null)
            {
                window.Owner = Window.GetWindow(base.AssociatedObject);
            }
            if (this.WindowStyle != null)
            {
                window.Style = this.WindowStyle;
            }
            if (this.WindowStartupLocation.HasValue)
            {
                window.WindowStartupLocation = this.WindowStartupLocation.Value;
            }
            return window;
        }
    }
}

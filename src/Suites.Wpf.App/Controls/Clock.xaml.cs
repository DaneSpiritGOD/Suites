using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Suites.Wpf.App.Controls
{
    /// <summary>
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : UserControl
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);

        public Clock()
        {
            InitializeComponent();

            _timer.Tick += Timer_Click;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        private void Timer_Click(object sender, EventArgs e)
        {
            timeText.Text = DateTime.Now.ToString(string.Format("yy{0}MM{1}dd{2}HH{3}mm{4}ss", "/", "/", " ", ":", ":"));
        }
    }
}

using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suites.Wpf.LiveCharts.Pie
{
    public static class Resources
    {
        public static readonly Func<ChartPoint, string> LabelPoint = cp => string.Format("{0} ({1:P})", cp.Y, cp.Participation);
    }
}

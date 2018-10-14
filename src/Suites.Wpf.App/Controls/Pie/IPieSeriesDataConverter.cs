using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suites.Wpf.LiveCharts.Pie
{
    public interface IPieSeriesDataConverter
    {
        bool CanConvert(IPieSeriesData data);
        PieSeries Convert(IPieSeriesData data);
    }
}

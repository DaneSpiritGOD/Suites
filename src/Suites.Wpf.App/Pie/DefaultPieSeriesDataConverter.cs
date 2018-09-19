using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Suites.Wpf.LiveCharts.Pie
{
    internal class DefaultPieSeriesDataConverter : PieSeriesDataConverterBase
    {
        public DefaultPieSeriesDataConverter() { }

        public override bool CanConvert(IPieSeriesData data) => true;

        public override PieSeries Convert(IPieSeriesData data)
        {
            return base.Convert(data);
        }
    }
}

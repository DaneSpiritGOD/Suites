using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts.Wpf;
using Suites.Wpf.LiveCharts.Pie;

namespace Suites.Wpf.LiveCharts.Pie
{
    public class CompositePieSeriesDataConverter : IPieSeriesDataConverter
    {
        IEnumerable<IPieSeriesDataConverter> _converters;
        IPieSeriesDataConverter _default;
        public CompositePieSeriesDataConverter(IEnumerable<IPieSeriesDataConverter> converters)
        {
            _converters = converters ?? throw new ArgumentNullException(nameof(converters) + " is null.");
            _default = new DefaultPieSeriesDataConverter();
        }

        public bool CanConvert(IPieSeriesData data) => true;

        public PieSeries Convert(IPieSeriesData data)
        {
            foreach (var converter in _converters)
            {
                if (converter.CanConvert(data))
                {
                    return converter.Convert(data);
                }
            }
            return _default.Convert(data);
        }
    }
}

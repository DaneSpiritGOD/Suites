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
    public abstract class PieSeriesDataConverterBase : IPieSeriesDataConverter
    {
        Func<ChartPoint, string> _labelPoint = cp => string.Format("{0} ({1:P})", cp.Y, cp.Participation);
        bool _showDataLabel = true;

        public virtual bool CanConvert(IPieSeriesData data)
        {
            throw new NotImplementedException();
        }

        public virtual PieSeries Convert(IPieSeriesData data)
        {
            return new PieSeries
            {
                Title = data.Title,
                Values = new ChartValues<ObservableValue>(data.Values),
                DataLabels = _showDataLabel,
                LabelPoint = _labelPoint,
            };
        }
    }
}
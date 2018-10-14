using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Suites.Wpf.LiveCharts.Pie
{
    public interface IPieChartDataSource
    {
        SeriesCollection SeriesCollection { get; }
    }

    public class PieChartDataSource : IPieChartDataSource
    {
        IEnumerable<IPieSeriesData> _psData;
        IPieSeriesDataConverter _converter;

        public PieChartDataSource(IEnumerable<IPieSeriesData> psData, IPieSeriesDataConverter converter)
        {
            _psData = psData ?? throw new ArgumentNullException(nameof(psData) + " is null.");
            _converter = converter ?? throw new ArgumentNullException(nameof(converter) + " is null.");
        }

        SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get
            {
                if (_seriesCollection == null)
                {
                    _seriesCollection = new SeriesCollection();
                    foreach (var data in _psData)
                    {
                        if (_converter.CanConvert(data))
                        {
                            _seriesCollection.Add(_converter.Convert(data));
                        }
                    }
                }
                return _seriesCollection;
            }
        }
    }
}

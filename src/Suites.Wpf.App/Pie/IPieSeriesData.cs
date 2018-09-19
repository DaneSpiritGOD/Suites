using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suites.Wpf.LiveCharts.Pie
{
    public interface IPieSeriesData
    {
        string Title { get; }
        IEnumerable<ObservableValue> Values { get; }
        string Description { get; }
    }
}
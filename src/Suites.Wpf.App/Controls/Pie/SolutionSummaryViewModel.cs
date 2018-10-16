using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suites.Wpf.App.Controls.Pie
{
    public class SolutionSummaryViewModel
    {
        public const string OKGroup = "OK";
        public const string NGGroup = "NG";

        public SolutionSummaryViewModel()
        {
            OkValues = new ChartValues<ObservableValue> { new ObservableValue(0) };
            NgValues = new ChartValues<ObservableValue> { new ObservableValue(0) };
        }

        public ChartValues<ObservableValue> OkValues { get; }
        public ChartValues<ObservableValue> NgValues { get; }
        //public void Add(ImageDto dto, string summaryGroup)
        //{
        //    switch (summaryGroup.ToUpper())
        //    {
        //        case NGGroup:
        //            NgValues[0].Value++;
        //            break;
        //        case OKGroup:
        //            OkValues[0].Value++;
        //            break;
        //        default:
        //            break;
        //    }
        //}

        public void Clear()
        {
            OkValues[0].Value = 0;
            NgValues[0].Value = 0;
        }
    }
}

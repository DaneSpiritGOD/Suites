using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suites.Wpf.App.Controls.Pie
{
    public interface ISimpleSeries
    {
        string Title { get; set; }
        ObservableDoubleValue Value { get; set; }
        string Color { get; set; }
    }

    public class SimpleSeries : ISimpleSeries
    {
        public string Title { get; set; }
        public ObservableDoubleValue Value { get; set; }
        public string Color { get; set; }
    }
}

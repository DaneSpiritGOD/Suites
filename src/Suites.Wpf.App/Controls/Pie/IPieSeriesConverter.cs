using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suites.Wpf.App.Controls.Pie
{
    public interface IPieSeriesConverter
    {
        string ExtractTitle(object data);
        string ExtractColor(object data);
        ObservableDoubleValue ExtractValue(object data);
    }

    public interface IPieSeriesConverter<T> : IPieSeriesConverter where T : class
    {
        string ExtractTitle(T data);
        string ExtractColor(T data);
        ObservableDoubleValue ExtractValue(T data);
    }

    public abstract class PieSeriesConverterBase<T> : IPieSeriesConverter<T> where T : class
    {
        public abstract string ExtractColor(T data);
        public abstract string ExtractTitle(T data);
        public abstract ObservableDoubleValue ExtractValue(T data);

        public virtual string ExtractColor(object data)
        {
            return ExtractColor((T)data);
        }

        public virtual string ExtractTitle(object data)
        {
            return ExtractTitle((T)data);
        }

        public virtual ObservableDoubleValue ExtractValue(object data)
        {
            return ExtractValue((T)data);
        }
    }
}

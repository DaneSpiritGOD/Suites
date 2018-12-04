using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Suites.Mvvm;
using Suites.Wpf.App.Converters;

namespace Suites.Wpf.App.Controls.Pie
{
    /// <summary>
    /// Interaction logic for SimplePie.xaml
    /// </summary>
    public partial class SimplePie : UserControl
    {
        static SimplePie()
        {
            Charting.For<ObservableDoubleValue>(Mappers.Xy<ObservableDoubleValue>().X((ObservableDoubleValue value, int index) => index).Y((ObservableDoubleValue value) => value.Value), SeriesOrientation.Horizontal);
            Charting.For<ObservableDoubleValue>(Mappers.Xy<ObservableDoubleValue>().X((ObservableDoubleValue value) => value.Value).Y((ObservableDoubleValue value, int index) => index), SeriesOrientation.Vertical);
        }

        public SimplePie()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SeriesSourceProperty =
            DependencyProperty.Register("Source", typeof(IEnumerable<ISimpleSeries>), typeof(SimplePie), new PropertyMetadata(onSourceChanged));
        public IEnumerable<ISimpleSeries> Source
        {
            get => (IEnumerable<ISimpleSeries>)GetValue(SeriesSourceProperty);
            set => SetValue(SeriesSourceProperty, value);
        }

        private static void onSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pie = (SimplePie)d;
            pie.onSourceChanged(e.OldValue, e.NewValue);
        }

        protected virtual void onSourceChanged(object oldItems, object newItems) => initial(oldItems, newItems);

        private void initial(object oldItems, object newItems)
        {
            var series = new SeriesCollection();
            var _items = newItems as IEnumerable<ISimpleSeries>;
            foreach (var item in _items)
            {
                var pieSeries = new PieSeries()
                {
                    Title = item.Title,
                    Fill = (new String2ColorBrushConverter().Convert(item.Color, null, null, CultureInfo.CurrentCulture)) as Brush,
                    Values = new ChartValues<ObservableDoubleValue> { item.Value },
                    DataLabels = true,
                    LabelPoint = Suites.Wpf.App.Controls.Pie.Resources.LabelPoint
                };
                series.Add(pieSeries);
            }

            _pie.Series = series;
        }
    }
}

using Suites.Wpf.MaterialDesign;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Suites.Wpf.App.Converters
{
    public class String2ColorBrushConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                      CultureInfo culture)
        {
            var color = value as string;
            try
            {
                if (ColorProvider.ColorBrushes.ContainsKey(color))
                {
                    return ColorProvider.ColorBrushes[color];
                }
                else
                {
                    return new ColorConverter().ConvertFrom(color);
                }
            }
            catch
            {
                return Brushes.Black;
            }
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

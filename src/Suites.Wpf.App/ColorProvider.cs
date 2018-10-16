using MaterialDesignColors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Suites.Wpf.MaterialDesign
{
    public static class ColorProvider
    {
        public static IReadOnlyDictionary<string, Color> Colors { get; }
        public static IReadOnlyDictionary<string, Brush> ColorBrushes { get; }
        public const string Accent = "accent";
        public const string Primary = "primary";
        static ColorProvider()
        {
            var provider = new SwatchesProvider();
            var dict = provider.Swatches
                .ToDictionary(x => $"{Primary} {x.Name}", y => y.ExemplarHue.Color, StringComparer.OrdinalIgnoreCase);

            var accentHueDict = provider.Swatches
                .Where(x => x.IsAccented)
                .ToDictionary(x => $"{Accent} {x.Name}", y => y.AccentExemplarHue.Color, StringComparer.OrdinalIgnoreCase);

            foreach (var item in accentHueDict)
            {
                dict[item.Key] = item.Value;
            }
            Colors = dict;

            var brushDict = new Dictionary<string, Brush>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in dict)
            {
                var brush = new SolidColorBrush(item.Value);
                brush.Freeze();
                brushDict[item.Key] = brush;
            }
            ColorBrushes = brushDict;
        }

        public static Color Red => Colors[$"{Accent} red"];
        public static Color Green => Colors[$"{Accent} green"];

        public static Brush RedBrush => ColorBrushes[$"{Accent} red"];
        public static Brush GreenBrush => ColorBrushes[$"{Accent} green"];
    }    
}

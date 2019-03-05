using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Suites.Wpf.Controls
{
    public class ImageMetaData
    {
        public int PixelWidth { get; internal set; }
        public int PixelHeight { get; internal set; }
        public int ChannelCount { get; internal set; }
        public Defect[] Defects { get; internal set; }
        public Rect FullRect => new Rect(new Point(0, 0), new Point(PixelWidth - 1, PixelHeight - 1));
        public int Stride => PixelWidth * ChannelCount;
        //public PixelFormat PF => GetChannelCount == 1 ? PixelFormats.Gray8 : PixelFormats.Bgr24;
    }
}

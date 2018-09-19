using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Media.Imaging
{
    public static class BitmapSourceExtensions
    {
        public static BitmapSource CreateFromFileContent(this Memory<byte> data)
        {
            NotTrueException.Assert(!data.IsEmpty, nameof(data));
            var img = new BitmapImage();
            var mem = new MemoryStream(data.ToArray());
            img.BeginInit();
            img.StreamSource = mem;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            img.Freeze();
            return img;
        }

        public static BitmapSource ShowROI(this BitmapSource source, Rect[] defectRects, int penThick = 3)
        {
            var brush = Brushes.Red;

            NamedNullException.Assert(source, nameof(source));
            NamedNullException.Assert(defectRects, nameof(defectRects));
            if (defectRects.Length == 0) return source;

            var dv = new DrawingVisual();
            var dc = dv.RenderOpen();
            dc.DrawImage(source, source.FullRect());
            foreach (var defect in defectRects)
            {
                dc.DrawRectangle(null, new Pen(brush, penThick), Rect.Intersect(defect, source.FullRect()));
            }
            dc.Close();

            var renderedBmp = new RenderTargetBitmap(
                source.PixelWidth,
                source.PixelHeight,
                96, 96,
                PixelFormats.Default);
            renderedBmp.Render(dv);
            return renderedBmp;
        }

        public static int GetChannelCount(this BitmapSource bs)
        {
            return (bs.Format.BitsPerPixel + 7) / 8;
        }

        public static int GetStride(this BitmapSource bs)
        {
            return (bs.PixelWidth * bs.Format.BitsPerPixel + 7) / 8;
        }

        public static PixelValue CopyPixel(this BitmapSource bs, int x, int y)
        {
            if (bs == null)
                throw new ArgumentNullException(nameof(bs) + " is null.");
            if (x >= bs.PixelWidth || x < 0)
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y >= bs.PixelHeight || y < 0)
                throw new ArgumentOutOfRangeException(nameof(y));

            var chn = bs.GetChannelCount();
            var value = new byte[chn];
            Array.Clear(value, 0, value.Length);

            bs.CopyPixels(
                new Int32Rect(x, y, 1, 1),
                value,
                (bs.Format.BitsPerPixel * bs.PixelWidth + 7) / 8,
                0);

            var pixelValue = new PixelValue();
            pixelValue.Alpha = PixelValue.AlphaDefault;
            switch (chn)
            {
                case 1:
                    pixelValue.Red = pixelValue.Green = pixelValue.Blue = value[0];
                    break;
                case 3:
                    pixelValue.Red = value[2];
                    pixelValue.Green = value[1];
                    pixelValue.Blue = value[0];
                    break;
                case 4:
                    pixelValue.Red = value[2];
                    pixelValue.Green = value[1];
                    pixelValue.Blue = value[0];
                    pixelValue.Alpha = value[3];
                    break;
                default:
                    throw new NotImplementedException();
            }

            return pixelValue;
        }

        public static byte[] CopyAllPixels(this BitmapSource bs)
        {
            var bytes = new byte[bs.GetStride() * bs.PixelHeight];
            bs.CopyPixels(bytes, bs.GetStride(), 0);
            return bytes;
        }

        public static Rect FullRect(this BitmapSource source) => new Rect(new Point(0, 0), new Point(source.PixelWidth - 1, source.PixelHeight - 1));

        public static PixelFormat CalcPixelFormat(this int channelCount)
        {
            switch (channelCount)
            {
                case 1:
                    return PixelFormats.Gray8;
                case 3:
                    return PixelFormats.Bgr24;
                case 4:
                    return PixelFormats.Bgr32;
                default:
                    throw new NotSupportedException();
            }
        }

        public static BitmapPalette DefaultPalette => BitmapPalettes.Gray256;
    }
}

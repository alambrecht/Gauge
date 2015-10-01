using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Gauge
{
    public static class GaugeImage
    {
        /// <summary>
        /// Create a gauge image
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size">Size in pixels. Must be 500 or less</param>
        /// <param name="format">Image Format</param>
        /// <returns></returns>
        public static byte[] Generate(this GaugeData data, int size, ImageFormat format)
        {
            if (size > 500) size = 500;
            if (!data.Ranges.Any()) return null;
            var min = data.Ranges.Min(t => t.MinValue);
            var max = data.Ranges.Max(t => t.MaxValue);
            var rangeTotal = max - min;
            var value = data.Value;
            var valueAngle = (((value - min) / rangeTotal) * 270) + 135;
            using (var bmp = new Bitmap(500, 500))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    var rec = new Rectangle(0, 0, 500, 500);
                    g.FillPie(new SolidBrush(Color.White), rec, 45, 90);
                    var startDeg = 135f;
                    foreach (var item in data.Ranges.OrderBy(o => o.MinValue))
                    {
                        g.FillPie(new SolidBrush(ColorTranslator.FromHtml(item.Color)), rec, startDeg,
                            (float)(((item.MaxValue - item.MinValue) / rangeTotal) * 270));
                        startDeg = (float)(startDeg + (((item.MaxValue - item.MinValue) / rangeTotal) * 270));
                    }
                    g.FillEllipse(new SolidBrush(Color.White), new Rectangle(100, 100, 300, 300));

                    using (var needle = Graphics.FromImage(bmp))
                    {
                        needle.TranslateTransform(250, 250);
                        needle.RotateTransform((float)valueAngle);
                        needle.TranslateTransform(-68, -39);
                        var myAssembly = Assembly.GetExecutingAssembly();
                        var myStream = myAssembly.GetManifestResourceStream("Gauge.needle.png");
                        needle.DrawImage(Image.FromStream(myStream), new PointF(0, 0));
                    }

                    g.FillRectangle(new SolidBrush(Color.Black), new RectangleF(new PointF(150, 375), new SizeF(200, 75)));
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(value.ToString(), new Font(FontFamily.GenericSansSerif, 40), Brushes.White, new PointF(250, 405), sf);
                    g.DrawString(data.Label, new Font(FontFamily.GenericSansSerif, 15), Brushes.White, new PointF(250, 435), sf);

                }
                using (var ms = new MemoryStream())
                {
                    bmp.MakeTransparent(Color.White);
                    var newBmp = new Bitmap(size, size);
                    using (var gr = Graphics.FromImage(newBmp))
                    {
                        gr.SmoothingMode = SmoothingMode.HighQuality;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        gr.DrawImage(bmp, new Rectangle(0, 0, size, size));
                    }

                    newBmp.Save(ms, format);
                    return ms.ToArray();
                }

            }
        }


    }
}

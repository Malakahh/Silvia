using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;

namespace SilviaCore
{
    public static class Extensions
    {
        public static BitmapImage ToWPFImageSource(this Image img)
        {
            BitmapImage bi = new BitmapImage();

            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;

                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();
            }

            return bi;
        }

        public static Vector ToVector(this System.Drawing.Point p)
        {
            return new Vector(p.X, p.Y);
        }

        public static Vector ToVector(this System.Windows.Point p)
        {
            return new Vector(p.X, p.Y);
        }
    }
}

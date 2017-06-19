using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Runtime.CompilerServices;

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

        /// <summary>
        /// Gets strictly the name of the executing assembly.
        /// </summary>
        /// <param name="t"></param>
        /// <returns>Name of the executing assembly, or empty string on error.</returns>
        public static string AssemblyName(this Type t)
        {
            string s = t.Assembly.ToString().Split(',')[0];

            return s ?? "";
        }

        //public static System.Windows.Forms.Screen GetScreen(this Window window)
        //{
        //    return System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(window).Handle);
        //}

        public static System.Windows.Media.SolidColorBrush ToBrush(this System.Drawing.Color c)
        {
            return System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B).ToBrush();
        }

        public static System.Windows.Media.SolidColorBrush ToBrush(this System.Windows.Media.Color c)
        {
            return new System.Windows.Media.SolidColorBrush(c);
        }

        public static System.Windows.Media.SolidColorBrush SetARGB(this System.Windows.Media.SolidColorBrush brush, uint argb)
        {
            System.Windows.Media.Color c = new System.Windows.Media.Color();

            c.A = (byte)(argb >> 8 * 3);
            c.R = (byte)(argb >> 8 * 2);
            c.G = (byte)(argb >> 8);
            c.B = (byte)argb;

            brush.Color = c;

            return brush;
        }

        public static System.Windows.Media.SolidColorBrush SetRGB(this System.Windows.Media.SolidColorBrush brush, uint rgb)
        {
            return brush.SetARGB(0xFF000000 | rgb);
        }

        public static System.Drawing.Color ToDrawingColor(this System.Windows.Media.Color c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public static System.Drawing.Color Complimentary(this System.Drawing.Color c)
        {
            return Color.FromArgb(c.A, 255 - c.R, 255 - c.G, 255 - c.B);
        }

        public static System.Drawing.Color SetAlpha(this System.Drawing.Color c, byte a)
        {
            return Color.FromArgb(a, c);
        }
    }
}

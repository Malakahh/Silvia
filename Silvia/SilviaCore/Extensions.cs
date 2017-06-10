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
    }
}

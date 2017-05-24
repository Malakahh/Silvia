using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.IO;

namespace SilviaCore
{
    public class ImageProcessing
    {
        public enum Blending { Multiplicative, Additive }

        public Image ApplyColorMask(Image orig, Color mask, Blending blending = Blending.Multiplicative)
        {
            Bitmap bmp = new Bitmap(orig);

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color c = bmp.GetPixel(x, y);

                    int A = 0;
                    int R = 0;
                    int G = 0;
                    int B = 0;

                    if (blending == Blending.Additive)
                    {
                        A = ColorComponentClamp(c.A + mask.A);
                        R = ColorComponentClamp(c.R + mask.R);
                        G = ColorComponentClamp(c.G + mask.G);
                        B = ColorComponentClamp(c.B + mask.B);
                    }
                    else if (blending == Blending.Multiplicative)
                    {
                        A = ColorComponentClamp(c.A * mask.A / 255);
                        R = ColorComponentClamp(c.R * mask.R / 255);
                        G = ColorComponentClamp(c.G * mask.G / 255);
                        B = ColorComponentClamp(c.B * mask.B / 255);
                    }

                    bmp.SetPixel(x, y, Color.FromArgb(A, R, G, B));
                }
            }

            return bmp;
        }

        private int ColorComponentClamp(int color)
        {
            if (color >= 255)
                return 255;
            else if (color <= 0)
                return 0;
            else
                return color;
        }
    }
}

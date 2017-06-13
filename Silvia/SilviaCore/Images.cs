using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using NLog;

namespace SilviaCore
{
    public static class Images
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static class HeaderIcons
        {
            internal static Color Normal = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
            internal static Color Highlight = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);

            public static Image OpenNormal;
            public static Image OpenHighlight;
        }

        public static class Icons
        {
            internal static Color Normal = Color.FromArgb(0x7C, 0xFF, 0xFF, 0xFF);
            internal static Color Highlight = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);

            public static Image PlusNormal;
            public static Image PlusHighlight;
        }

        public static void Init()
        {
            logger.Trace("Loading and processing images...");

            string imgPath = Directory.GetCurrentDirectory() + "\\Assets\\";
            ImageProcessing imgProc = new ImageProcessing();
            
            string headerIconOpenPath = imgPath + "headerIconOpen.png";
            logger.Trace(headerIconOpenPath);
            Images.HeaderIcons.OpenNormal = imgProc.ApplyColorMask(
                Bitmap.FromFile(headerIconOpenPath),
                HeaderIcons.Normal);
            Images.HeaderIcons.OpenHighlight = imgProc.ApplyColorMask(
                Bitmap.FromFile(headerIconOpenPath),
                HeaderIcons.Highlight);

            string plusIconPath = imgPath + "iconPlus.png";
            logger.Trace(plusIconPath);
            Images.Icons.PlusNormal = imgProc.ApplyColorMask(
                Bitmap.FromFile(plusIconPath),
                Icons.Normal);
            Images.Icons.PlusHighlight = imgProc.ApplyColorMask(
                Bitmap.FromFile(plusIconPath),
                Icons.Highlight);

            logger.Trace("Images loaded and processsed");
        }
    }
}

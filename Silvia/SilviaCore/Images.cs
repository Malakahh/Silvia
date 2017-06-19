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
            public static Image OpenNormal;
            public static Image OpenHighlight;
        }

        public static class Icons
        {
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
                Themes.ThemeSettings.Instance.HeaderIconNormal);
            Images.HeaderIcons.OpenHighlight = imgProc.ApplyColorMask(
                Bitmap.FromFile(headerIconOpenPath),
                Themes.ThemeSettings.Instance.HeaderIconHighlight);

            string plusIconPath = imgPath + "iconPlus.png";
            logger.Trace(plusIconPath);
            Images.Icons.PlusNormal = imgProc.ApplyColorMask(
                Bitmap.FromFile(plusIconPath),
                Themes.ThemeSettings.Instance.IconNormal);
            Images.Icons.PlusHighlight = imgProc.ApplyColorMask(
                Bitmap.FromFile(plusIconPath),
                Themes.ThemeSettings.Instance.IconHighlight);

            logger.Trace("Images loaded and processsed");
        }
    }
}

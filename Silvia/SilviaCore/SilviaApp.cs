using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace SilviaCore
{
    public class SilviaApp
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public const string AppName = "SiLvia";
        public const string AppNameFull = AppName + " Personal Assistant";

        private static bool running = false;

#region Events
        public static event Action OnApplicationInit;
        public static event Action OnApplicationClosing;
#endregion

        public static void Init()
        {
            logger.Trace("Application Init");

            PluginLoader.LoadPlugins();

            OnApplicationInit?.Invoke();

            running = true;
            while(running)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }

        public static void Close()
        {
            running = false;

            OnApplicationClosing?.Invoke();

            logger.Trace("Application close");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SilviaCore.Commands;
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

            InitBaseCommands();

            Settings.CreatePluginSettingsDir("SilviaCore");
            Settings.LoadSettings();

            Themes.ThemeSettings.Init();
            Images.Init();

            PluginLoader.LoadPlugins();

            //Call Plugin.OnLoad
            foreach (Plugin p in PluginLoader.Plugins.Values)
            {
                p.OnLoad();
            }

            OnApplicationInit?.Invoke();

            running = true;
            while(running)
            {
                System.Windows.Forms.Application.DoEvents();
            }

            OnApplicationClosing?.Invoke();

            Settings.SaveSettings();
        }

        public static void Close()
        {
            running = false;

            logger.Trace("Application close");
        }

        private static void InitBaseCommands()
        {
            CmdHandler.AddCmd(new Command(
                "^close$",
                (args) => {
                    Close();
                }));

            CmdHandler.AddCmd(new Command(
                "^exit$",
                (args) => {
                    Close();
                }));
        }
    }
}

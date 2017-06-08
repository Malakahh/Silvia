using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using NLog;

namespace SilviaCore
{
    public class PluginLoader
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static string pluginPath = Directory.GetCurrentDirectory() + "\\Plug-ins";

        private static Dictionary<string, Plugin> plugins;
        public static Dictionary<string, Plugin> Plugins
        {
            get
            {
                return new Dictionary<string, Plugin>(plugins);
            }
        }

        public static void AddPluginDll(string path)
        {
            Assembly asm = Assembly.LoadFile(path);

            foreach (Type t in asm.GetTypes())
            {
                if (t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Plugin)))
                {
                    Plugin p = asm.CreateInstance(t.FullName) as Plugin;

                    if (p != null)
                    {
                        string dllName = path.Split('\\').Last();

                        logger.Info("Loaded plugin: {0}", dllName);

                        plugins.Add(dllName, p);

                        Settings.CreatePluginSettingsDir(p.PluginName);

                        p.OnLoad();
                    }
                }
            }
        }

        internal static void LoadPlugins()
        {
            CreatePluginDirectory();

            string[] potentialPlugins = Directory.GetFiles(pluginPath, "*.dll", SearchOption.AllDirectories);
            logger.Info("Plugin DLLs found: {0}", potentialPlugins.Length);
            
            plugins = new Dictionary<string, Plugin>(potentialPlugins.Length);

            foreach (string filePath in potentialPlugins)
            {
                AddPluginDll(filePath);
            }

            logger.Info("Plugins found: {0}", plugins.Count);
        }

        private static void CreatePluginDirectory()
        {
            if (!Directory.Exists(pluginPath))
            {
                Directory.CreateDirectory(pluginPath);
            }
        }
    }
}

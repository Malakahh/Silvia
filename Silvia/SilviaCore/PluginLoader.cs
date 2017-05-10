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
    class PluginLoader
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static string pluginPath = Directory.GetCurrentDirectory() + "\\Plug-ins";

        internal static List<Plugin> Plugins;

        internal static void LoadPlugins()
        {
            CreatePluginDirectory();

            string[] potentialPlugins = Directory.GetFiles(pluginPath, "*.dll", SearchOption.AllDirectories);
            logger.Info("Plugin DLLs found: {0}", potentialPlugins.Length);

            Plugins = new List<Plugin>(potentialPlugins.Length);

            foreach (string filePath in potentialPlugins)
            {
                Assembly asm = Assembly.LoadFile(filePath);

                foreach (Type t in asm.GetTypes())
                {
                    if (t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Plugin)))
                    {
                        Plugin p = asm.CreateInstance(t.FullName) as Plugin;

                        if (p != null)
                        {
                            logger.Info("Loaded plugin: {0}", filePath.Split('\\').Last());

                            Plugins.Add(p);
                        }
                    }
                }
            }

            logger.Info("Plugins found: {0}", Plugins.Count);
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

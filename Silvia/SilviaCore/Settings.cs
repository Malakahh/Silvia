using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace SilviaCore
{
    public abstract class Settings
    {
        private static string settingsPath = "";
        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal static void CreatePluginSettingsDir(string pluginName)
        {
            if (settingsPath == "")
            {
                settingsPath = Directory.GetCurrentDirectory() + "\\Settings";
                if (!Directory.Exists(settingsPath))
                {
                    Directory.CreateDirectory(settingsPath);
                }
            }

            string pluginSettingsPath = settingsPath + "\\" + pluginName;
            if (!Directory.Exists(pluginSettingsPath))
            {
                Directory.CreateDirectory(pluginSettingsPath);
            }
        }

        public static void Save(Settings s)
        {
            string assemblyName = s.GetType().AssemblyName() + ".dll";
            string path = "";

            if (assemblyName != "" && PluginLoader.Plugins.ContainsKey(assemblyName))
            {
                string pluginName = PluginLoader.Plugins[assemblyName].PluginName;
                path = settingsPath + "\\" + pluginName + "\\" + s.GetType().Name + ".json";
            }
            else
            {
                path = settingsPath + "\\" + s.GetType().Name + ".json";
            }

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(JsonConvert.SerializeObject(s));
            }

            logger.Trace("Saved settings: " + path + "...");
        }

        public static T Load<T>()
        {
            string assemblyName = typeof(T).AssemblyName() + ".dll";
            string path = "";

            if (assemblyName != "" && PluginLoader.Plugins.ContainsKey(assemblyName))
            {
                string pluginName = PluginLoader.Plugins[assemblyName].PluginName;
                path = settingsPath + "\\" + pluginName + "\\" + typeof(T).Name + ".json";
            }
            else
            {
                path = settingsPath + "\\" + typeof(T).Name + ".json";
            }

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                T s = JsonConvert.DeserializeObject<T>(json);

                logger.Trace("Loaded settings: " + path + ".dll");

                return s;
            }
            else
            {
                return default(T);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using NLog;
using System.Runtime.CompilerServices;

namespace SilviaCore
{
    public abstract class Settings
    {
        private static string settingsPath = "";
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Dictionary -> Assembly.Namespaces.Classname, json
        /// </summary>
        private static Dictionary<string, List<object>> allSettings = new Dictionary<string, List<object>>();

        static Settings()
        {
            if (settingsPath == "")
            {
                settingsPath = Directory.GetCurrentDirectory() + "\\Settings";
                if (!Directory.Exists(settingsPath))
                {
                    Directory.CreateDirectory(settingsPath);
                }
            }
        }

        public Settings()
        {
            string fullName = GetFullNameOfSettings(this.GetType());

            if (!allSettings.ContainsKey(fullName))
            {
                allSettings.Add(fullName, new List<object>());
            }

            allSettings[fullName].Add(this);
        }

        internal static void CreatePluginSettingsDir(string pluginName)
        {
            string pluginSettingsPath = settingsPath + "\\" + pluginName;
            if (!Directory.Exists(pluginSettingsPath))
            {
                Directory.CreateDirectory(pluginSettingsPath);
            }
        }

        internal static void LoadSettings()
        {
            string[] directories = Directory.GetDirectories(settingsPath);

            foreach (string dir in directories)
            {
                string[] filePaths = Directory.GetFiles(dir);
                foreach (string fp in filePaths)
                {
                    //object o = Load(fp);

                    //if (o != null)
                    //{
                    //    string fullClassName = fp.Split('\\').Last();

                    //    if (!allSettings.ContainsKey(fullClassName))
                    //    {
                    //        allSettings.Add(fullClassName, new List<object>());
                    //    }

                    //    allSettings[fullClassName].Add(o);
                    //}

                    Load(fp);
                }
            }
        }

        internal static void SaveSettings()
        {
            foreach (var kv in allSettings)
            {
                Save(kv.Value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetFullNameOfSettings(Type t)
        {
            return t.AssemblyName() + "." + t.FullName;
        }

        public static T GetSettings<T>(int index)
        {
            string fullName = GetFullNameOfSettings(typeof(T));
            if (allSettings.ContainsKey(fullName) && allSettings[fullName].Count > 0)
            {
                if (TryCast<T>(allSettings[fullName][index], out T ret))
                {
                    return ret;
                }
            }

            return default(T);
        }

        private static bool TryCast<T>(object toCast, out T result)
        {
            if (toCast is T)
            {
                result = (T)toCast;
                return true;
            }

            result = default(T);
            return false;
        }

        private static void Save(List<object> list)
        {
            if (list.Count <= 0)
                return;

            object o = list[0];
            string type = o.GetType().FullName;
            string path = settingsPath + "\\" + o.GetType().AssemblyName() + "\\" + GetFullNameOfSettings(o.GetType()) + ".json";

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(JsonConvert.SerializeObject(o, typeof(object), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented }));
            }

            logger.Trace("Saved settings: " + path + "...");
        }

        private static object Load(string path)
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                object obj;

                try
                {
                    obj = JsonConvert.DeserializeObject(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
                catch (Newtonsoft.Json.JsonSerializationException ex)
                {
                    logger.Error(ex.ToString());
                    return null;
                }

                logger.Trace("Loaded settings: " + path + ".dll");

                return obj;
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SilviaBootstrapper
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SilviaCore.PluginLoader.AddDllToPotentialPlugins(Directory.GetCurrentDirectory() + "\\SilviaGUI.dll");
            SilviaCore.SilviaApp.Init();
        }
    }
}

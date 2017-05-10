using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilviaBootstrapper
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SilviaGUI.SilviaGUI hi = new SilviaGUI.SilviaGUI();

            SilviaCore.SilviaApp.Init();
        }
    }
}

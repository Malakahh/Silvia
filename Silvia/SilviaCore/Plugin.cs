using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SilviaCore
{
    public abstract class Plugin
    {
        public abstract string PluginName { get; }

        public virtual void OnLoad()
        {

        }

        public virtual void Refresh()
        {

        }

        public virtual Window GetWindow()
        {
            return null;
        }
    }
}

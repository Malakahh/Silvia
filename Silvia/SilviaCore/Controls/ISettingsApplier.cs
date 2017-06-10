using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilviaCore.Controls
{
    public interface ISettingsApplier<T>
    {
        void ApplySettings(T settings = default(T)); 
    }
}

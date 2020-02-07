using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConvertibleTray
{
    class KeyboardDisabler
    {

        //https://stackoverflow.com/questions/20841501/blockinput-method-doesnt-work-on-windows-7
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BlockInput(bool fBlockIt);
    }
}

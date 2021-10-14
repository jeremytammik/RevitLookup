using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitLookup.Snoop
{
    class ModelessWindowHandle : IWin32Window
    {
        public IntPtr Handle { get; }


        public ModelessWindowHandle()
        {
            Process process = Process.GetCurrentProcess();
            Handle = process.MainWindowHandle;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitLookup.Snoop
{
    internal class ModelessWindowHandle : IWin32Window
    {
        public static IntPtr RevitMainWindowHandle { get; set; }
        public IntPtr Handle { get; }


        public ModelessWindowHandle()
        {            
            Handle = RevitMainWindowHandle;
        }

        public ModelessWindowHandle(Form form) : this()
        {
            // That does not work very well
            //Handle = form.Handle;
        }

        [DllImport("USER32.DLL")]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void BringRevitToFront()
        {
            SetForegroundWindow(RevitMainWindowHandle);
        }
    }
}

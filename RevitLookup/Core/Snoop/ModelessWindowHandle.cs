using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RevitLookup.Core.Snoop
{
    internal class ModelessWindowHandle : IWin32Window
    {
        public ModelessWindowHandle()
        {
            Handle = RevitMainWindowHandle;
        }

        public ModelessWindowHandle(Form form) : this()
        {
            // That does not work very well
            //Handle = form.Handle;
        }

        public static IntPtr RevitMainWindowHandle { get; set; }
        public IntPtr Handle { get; }

        [DllImport("USER32.DLL")]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void BringRevitToFront()
        {
            SetForegroundWindow(RevitMainWindowHandle);
        }
    }
}
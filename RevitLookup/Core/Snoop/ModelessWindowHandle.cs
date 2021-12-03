using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RevitLookup.Core.Snoop
{
    internal class ModelessWindowHandle : IWin32Window
    {
        private static IntPtr _revitMainWindowHandle;

        public ModelessWindowHandle()
        {
            Handle = _revitMainWindowHandle;
        }

        public IntPtr Handle { get; }

        public static void SetHandler(IntPtr handler)
        {
            _revitMainWindowHandle = handler;
        }

        [DllImport("USER32.DLL")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void BringRevitToFront()
        {
            SetForegroundWindow(_revitMainWindowHandle);
        }
    }
}
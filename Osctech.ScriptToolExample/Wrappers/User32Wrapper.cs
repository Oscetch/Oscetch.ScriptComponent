using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Osctech.ScriptToolExample.Wrappers
{
    public static class User32Wrapper
    {
        private const int WM_SETREDRAW = 0x0b;

        public static void BeginUpdate(IntPtr handle)
        {
            SendMessage(handle, WM_SETREDRAW, (IntPtr)0, IntPtr.Zero);
        }
        public static void EndUpdate(IntPtr handle)
        {
            SendMessage(handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
}

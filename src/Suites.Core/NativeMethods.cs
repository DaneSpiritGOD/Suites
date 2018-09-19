using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Suites.Core
{
    [SecurityCritical, SuppressUnmanagedCodeSecurity]
    public static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern bool QueryPerformanceCounter(out long value);
        [DllImport("kernel32.dll")]
        public static extern bool QueryPerformanceFrequency(out long value);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", ExactSpelling = true)]
        public static extern void CopyMemory(IntPtr pdst, IntPtr psrc, int cb);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace GazoView.Functions
{
    class WorkingSet
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetProcessWorkingSetSize(IntPtr procHandle, IntPtr min, IntPtr max);
        private static Process proc = null;

        /// <summary>
        /// 非同期にバックグラウンド実行し、随時Shrink
        /// </summary>
        /// <returns></returns>
        public static async Task ShurnkAsync()
        {
            int _interval = 10000;

            await Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(_interval);
                    proc = Process.GetCurrentProcess();
                    SetProcessWorkingSetSize(proc.Handle, new IntPtr(-1L), new IntPtr(-1L));
                }
            });
        }

        /// <summary>
        /// 手動でShrink
        /// </summary>
        public static void Shrink()
        {
            proc = Process.GetCurrentProcess();
            SetProcessWorkingSetSize(proc.Handle, new IntPtr(-1L), new IntPtr(-1L));
        }
    }
}

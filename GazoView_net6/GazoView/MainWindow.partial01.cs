using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace GazoView
{
    /// <summary>
    /// WindowChrome使用環境での、ウィンドウ最大化時、コントロールの端が途切れないようにする為の対応
    /// </summary>
    public partial class MainWindow : Window
    {
        #region For fullscreen

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MonitorInformation lpmi);

        [StructLayout(LayoutKind.Sequential)]
        internal struct NativePoint
        {
            internal int X;
            internal int Y;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        internal struct MinMaxInformation
        {
            internal NativePoint ptReserved;
            internal NativePoint ptMaxSize;
            internal NativePoint ptMaxPosition;
            internal NativePoint ptMinTrackSize;
            internal NativePoint ptMaxTrackSize;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        internal struct Rectangle
        {
            internal int Left;
            internal int Top;
            internal int Right;
            internal int Bottom;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        internal class MonitorInformation
        {
            internal int cbSize = Marshal.SizeOf(typeof(MonitorInformation));
            internal Rectangle rcMonitor = new Rectangle();
            internal Rectangle rcWork = new Rectangle();
            internal int dwFlags = 0;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr handle = (new WindowInteropHelper(this)).Handle;
            HwndSource.FromHwnd(handle).AddHook(WinProc);
        }

        private IntPtr WinProc(IntPtr hwnd, int message, IntPtr wparam, IntPtr lparam,
          ref bool handled)
        {
            switch (message)
            {
                case 0x0024:
                    handled = WmGetMinMaxInfo(hwnd, lparam, this);
                    break;
            }
            return IntPtr.Zero;
        }

        private static bool WmGetMinMaxInfo(IntPtr hwnd, IntPtr lparam, Window window)
        {
            MinMaxInformation mmi =
                (MinMaxInformation)Marshal.PtrToStructure(lparam, typeof(MinMaxInformation));
            IntPtr monitor = MonitorFromWindow(hwnd, 2);
            if (monitor == IntPtr.Zero)
            {
                return false;
            }

            MonitorInformation monitorInformation = new MonitorInformation();
            if (!GetMonitorInfo(monitor, monitorInformation))
            {
                return false;
            }

            Rectangle workArea = monitorInformation.rcWork;
            Rectangle monitorArea = monitorInformation.rcMonitor;
            mmi.ptMaxPosition.X = Math.Abs(workArea.Left - monitorArea.Left);
            mmi.ptMaxPosition.Y = Math.Abs(workArea.Top - monitorArea.Top);
            mmi.ptMaxSize.X = Math.Abs(workArea.Right - workArea.Left);
            mmi.ptMaxSize.Y = Math.Abs(workArea.Bottom - workArea.Top);

            Point magnification = GetDeviceToLogicalCoefficient(window);

            if (!double.IsInfinity(window.MinWidth) && !double.IsNaN(window.MinWidth))
            {
                mmi.ptMinTrackSize.X = (int)(window.MinWidth * magnification.X);
            }
            if (!double.IsInfinity(window.MinHeight) && !double.IsNaN(window.MinHeight))
            {
                mmi.ptMinTrackSize.Y = (int)(window.MinHeight * magnification.Y);
            }
            if (!double.IsInfinity(window.MaxWidth) && !double.IsNaN(window.MaxWidth))
            {
                mmi.ptMaxTrackSize.X = (int)(window.MaxWidth * magnification.X);
            }
            if (!double.IsInfinity(window.MaxHeight) && !double.IsNaN(window.MaxHeight))
            {
                mmi.ptMaxTrackSize.Y = (int)(window.MaxHeight * magnification.Y);
            }

            Marshal.StructureToPtr(mmi, lparam, true);
            return true;
        }

        internal static Point GetDeviceToLogicalCoefficient(Window window)
        {
            PresentationSource presentationSource = PresentationSource.FromVisual(window);
            if (presentationSource == null || presentationSource.CompositionTarget == null)
            {
                return new Point(1.0, 1.0);
            }

            return new Point
            {
                X = presentationSource.CompositionTarget.TransformToDevice.M11,
                Y = presentationSource.CompositionTarget.TransformToDevice.M22
            };
        }

        #endregion
    }
}

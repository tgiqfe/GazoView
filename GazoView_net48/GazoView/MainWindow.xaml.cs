using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using GazoView.Config;
using System.Windows.Threading;
using System.Collections.Specialized;
using GazoView.Functions;

namespace GazoView
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
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

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = Item.Data;

            //  拡縮/トリミングモードOFF
            ChangeScalingMode(toScaling: false);
            ChangeTrimmingMode(toTrimming: false);

            //  他プロセス待ち受け
            PipeMessage.Start(this).ConfigureAwait(false);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateImage(moveCount: 0);
        }

        #region Caption Button

        /// <summary>
        /// 最小化ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 最大化ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                maximizeButton.Content = "1";
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                maximizeButton.Content = "2";
            }
        }

        /// <summary>
        /// 終了ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 最小化/最大化時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Minimized:
                    WorkingSet.Shrink();
                    StopAutoImageStoreItems();
                    break;
                case WindowState.Normal:
                case WindowState.Maximized:
                    StartAutoImageStoreItems();
                    break;
            }
        }

        #endregion

        /// <summary>
        /// 画像更新
        /// </summary>
        /// <param name="moveCount"></param>
        public void UpdateImage(int moveCount)
        {
            Item.ImageStore.CurrentIndex += moveCount;
            ImageSource image = Item.ImageStore.GetCurrentImageSource(mainImage);

            if (image != null)
            {
                if (Item.Data.State.ScalingMode)
                {
                    double scale = rate.Value / 100;

                    canvas.Width = Item.ImageStore.CurrentWidth * scale;
                    canvas.Height = Item.ImageStore.CurrentHeight * scale;
                }
                else
                {
                    if (Item.ImageStore.CurrentWidth < this.ActualWidth && Item.ImageStore.CurrentHeight < this.ActualHeight)
                    {
                        canvas.Width = Item.ImageStore.CurrentWidth;
                        canvas.Height = Item.ImageStore.CurrentHeight;
                    }
                    else
                    {
                        canvas.Width = double.NaN;
                        canvas.Height = double.NaN;
                    }
                }

                mainImage.Source = image;
                imageTitle.Text = Item.ImageStore.GetCurrentImageTitle();
            }
            else
            {
                mainImage.Source = null;
                imageTitle.Text = "";
            }

            StartAutoImageStoreItems();
            UpdateImageInfo();
        }

        /// <summary>
        /// ImageInfoを更新
        /// </summary>
        private void UpdateImageInfo()
        {
            if (imageInfoPanel.Visibility == Visibility.Visible)
            {
                if (Item.ImageStore.CurrentImageSource == null)
                {
                    info_ImageName.Text = "ファイル名: -";
                    info_ImagePath.Text = "ファイルパス: -";
                    info_ImageExtension.Text = "拡張子: -";
                    info_ImageSize.Text = "ファイルサイズ: -";
                    info_ImageLength.Text = "画像サイズ: -";
                    info_ImageModDate.Text = "更新日時: -";

                    fileCount.Text = "( - / - )";
                }
                else
                {
                    FileInfo fi = new FileInfo(Item.ImageStore.CurrentPath);

                    string[] suffixies = { "", "K", "M", "G", "T" };
                    int index = 0;
                    long fileSize = fi.Length;
                    while (fileSize >= 1024)
                    {
                        fileSize /= 1024;
                        index++;
                    }
                    string imageFileSize = string.Format("{0:#,0} {1}B",
                        fileSize,
                        index < suffixies.Length ? suffixies[index] : "-");

                    info_ImageName.Text = "ファイル名: " + Path.GetFileName(Item.ImageStore.CurrentPath);
                    info_ImagePath.Text = "ファイルパス: " + Item.ImageStore.CurrentPath;
                    info_ImageExtension.Text = "拡張子: " + Item.ImageStore.CurrentExtension;
                    info_ImageSize.Text = "ファイルサイズ: " + imageFileSize;
                    info_ImageLength.Text = "画像サイズ: " + string.Format("{0} × {1}", Item.ImageStore.CurrentWidth, Item.ImageStore.CurrentHeight);
                    info_ImageModDate.Text = "更新日時: " + fi.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");

                    fileCount.Text = string.Format("( {0} / {1} )",
                        Item.ImageStore.CurrentIndex + 1, Item.ImageStore.Items.Count);
                }
            }
        }

        /// <summary>
        /// 現在表示している画像を削除
        /// </summary>
        private void DeleteImage(bool forceDelete)
        {
            StopAutoImageStoreItems();

            if (Item.ImageStore.Items.Count > 0)
            {
                int afterIndex = Item.ImageStore.CurrentIndex;
                if (afterIndex >= Item.ImageStore.Items.Count - 1)
                {
                    afterIndex--;
                }

                if (forceDelete)
                {
                    File.Delete(Item.ImageStore.CurrentPath);
                }
                else
                {
                    FileSystem.DeleteFile(Item.ImageStore.CurrentPath,
                        UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }

                Item.ImageStore.Items.RemoveAt(Item.ImageStore.CurrentIndex);
                Item.ImageStore.CurrentIndex = afterIndex;
                UpdateImage(moveCount: 0);
            }

            StartAutoImageStoreItems();
        }

        /// <summary>
        /// ウィンドウサイズ変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ImageSource image = Item.ImageStore.CurrentImageSource;
            if (image != null)
            {
                if (Item.Data.State.ScalingMode)
                {
                    double scale = rate.Value / 100;

                    canvas.Width = Item.ImageStore.CurrentWidth * scale;
                    canvas.Height = Item.ImageStore.CurrentHeight * scale;
                }
                else
                {
                    if (Item.ImageStore.CurrentWidth < this.ActualWidth && Item.ImageStore.CurrentHeight < this.ActualHeight)
                    {
                        canvas.Width = Item.ImageStore.CurrentWidth;
                        canvas.Height = Item.ImageStore.CurrentHeight;
                    }
                    else
                    {
                        canvas.Width = double.NaN;
                        canvas.Height = double.NaN;
                    }
                }
            }
        }

        #region Auto ItemsUpdate

        FileSystemWatcher _watcher = null;

        /// <summary>
        /// 自動更新スレッドを開始
        /// </summary>
        private void StartAutoImageStoreItems()
        {
            if (Item.ImageStore.IsAllImage)
            {
                if (_watcher == null)
                {
                    _watcher = new FileSystemWatcher();
                    _watcher.Path = Item.ImageStore.CurrentParentDirectory;
                    _watcher.NotifyFilter = NotifyFilters.FileName;
                    _watcher.Created += (_source, _e) => { Item.ImageStore.UpdateItems(); };
                    _watcher.Deleted += (_source, _e) => { Item.ImageStore.UpdateItems(); };
                }
                _watcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// 自動更新スレッドを停止
        /// </summary>
        private void StopAutoImageStoreItems()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
                _watcher = null;
            }
        }

        #endregion
    }
}

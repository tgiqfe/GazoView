using GazoView.Lib.Functions;
using GazoView.Lib.Panel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace GazoView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //  Esc key event
        private DispatcherTimer _keyHoldTimer;
        private Key? _currentHeldKey;
        private const int KeyHoldDelay = 300;

        public MainWindow()
        {
            InitializeComponent();
            Init();

            //  Load後1秒後にMainWindow loaded flagをtrueにする。これにより、MainWindowの初期化が完了したとみなす。
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Item.IsInitialized = true;
                });
            });
        }

        private void Init()
        {
            Item.MainWindow = this;
            this.DataContext = Item.BindingParam;

            //  Add event (for drag move).
            EventManager.RegisterClassHandler(
                typeof(AdvancedScrollViewer),
                FrameworkElement.MouseLeftButtonDownEvent,
                new MouseButtonEventHandler((sender, e) =>
                {
                    if (!Item.BindingParam.Trimming.IsTrimmingMode) this.DragMove();
                }));
        }

        #region Key events

        /// <summary>
        /// Key down event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Detects key repeat. Ignores if Esc is already pressed.
            if (e.IsRepeat && _currentHeldKey.HasValue) return;

            switch (e.Key)
            {
                case Key.Escape:
                    if (Item.BindingParam.RenameBox.IsVisible) return;
                    if (Item.BindingParam.MessageDialog.IsVisible) return;
                    _currentHeldKey = e.Key;
                    _keyHoldTimer = new DispatcherTimer();
                    _keyHoldTimer.Interval = TimeSpan.FromMilliseconds(KeyHoldDelay);
                    _keyHoldTimer.Tick += (sender, e) =>
                    {
                        if (_currentHeldKey.HasValue)
                        {
                            FolderWindow.Open(Item.BindingParam.Images.Current.Parent, Item.BindingParam.Images.Current.FileName);
                        }
                        if (!this.IsFocused)
                        {
                            Application.Current.Shutdown();
                        }
                    };
                    _keyHoldTimer.Start();
                    break;
                case Key.Left:
                    if (Item.BindingParam.RenameBox.IsVisible) return;
                    if(Item.BindingParam.MessageDialog.IsVisible) return;
                    Item.BindingParam.Images.Index--;
                    Item.BindingParam.Images.UpdateImage();
                    break;
                case Key.Right:
                    if (Item.BindingParam.RenameBox.IsVisible) return;
                    if (Item.BindingParam.MessageDialog.IsVisible) return;
                    Item.BindingParam.Images.Index++;
                    Item.BindingParam.Images.UpdateImage();
                    break;
                case Key.Home:
                    if (Item.BindingParam.RenameBox.IsVisible) return;
                    if (Item.BindingParam.MessageDialog.IsVisible) return;
                    Item.BindingParam.Images.Index = 0;
                    Item.BindingParam.Images.UpdateImage();
                    break;
                case Key.End:
                    if (Item.BindingParam.RenameBox.IsVisible) return;
                    if (Item.BindingParam.MessageDialog.IsVisible) return;
                    Item.BindingParam.Images.Index = Item.BindingParam.Images.Length - 1;
                    Item.BindingParam.Images.UpdateImage();
                    break;
                case Key.T:
                    //  Switch trimming mode.
                    MoveTriangleLayer.LeftTriangleArea.Visibility = Visibility.Collapsed;
                    MoveTriangleLayer.RightTriangleArea.Visibility = Visibility.Collapsed;
                    Item.BindingParam.Trimming.SwitchMode();
                    break;
                case Key.G:
                    //  Start trimming.
                    if (Item.BindingParam.Trimming.IsTrimmingMode)
                    {
                        //Item.BindingParam.Trimming.StartTrimming();
                        Item.BindingParam.MessageDialog.ShowTrimmingWindow();
                    }
                    break;
                case Key.O:
                    //  Open or close folder path.
                    if (SpecialKeyStatus.IsCtrlPressed())
                    {
                        FolderWindow.Close(Item.BindingParam.Images.Current.Parent);
                    }
                    else
                    {
                        FolderWindow.Open(Item.BindingParam.Images.Current.Parent, Item.BindingParam.Images.Current.FileName);
                    }
                    break;
                case Key.F2:
                    Item.BindingParam.RenameBox.SwitchMode();
                    break;
                case Key.Delete:
                    Item.BindingParam.MessageDialog.ShowDeleteWindow();
                    break;
                case Key.Z:
                    if (SpecialKeyStatus.IsCtrlPressed())
                    {
                        Item.BindingParam.MessageDialog.ShowRestoreWindow();
                    }
                    break;
            }
        }

        /// <summary>
        /// Key up event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            // The timer stops when the key is released.
            if (_keyHoldTimer != null && _currentHeldKey.HasValue && _currentHeldKey.Value == e.Key)
            {
                _keyHoldTimer.Stop();
                _currentHeldKey = null;
            }

            switch (e.Key)
            {
                case Key.Escape:
                    //  Close rename box if F2 is released.
                    if (Item.BindingParam.RenameBox.IsVisible)
                    {
                        Item.BindingParam.RenameBox.HideWindow();
                        Debug.WriteLine("Rename box closed.");
                        return;
                    }
                    if (Item.BindingParam.MessageDialog.IsVisible)
                    {
                        Item.BindingParam.MessageDialog.HideWindow();
                        Debug.WriteLine("Delete message box closed.");
                        return;
                    }
                    Application.Current.Shutdown();
                    break;
            }
        }

        #endregion

        #region Navigation triangles

        /// <summary>
        /// Show navigation triangles on mouse move.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GlobalGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (Item.BindingParam.Images == null || Item.BindingParam.Images.Length <= 1) return;
            if (Item.BindingParam.Trimming.IsTrimmingMode) return;

            Point position = e.GetPosition(GlobalGrid);
            double windowWidth = GlobalGrid.ActualWidth;
            double leftThreshold = 100;
            double rightThreshold = windowWidth - 100;

            if (position.X < leftThreshold)
            {
                MoveTriangleLayer.LeftTriangleArea.Visibility = Visibility.Visible;
                MoveTriangleLayer.RightTriangleArea.Visibility = Visibility.Collapsed;
            }
            else if (position.X > rightThreshold)
            {
                MoveTriangleLayer.LeftTriangleArea.Visibility = Visibility.Collapsed;
                MoveTriangleLayer.RightTriangleArea.Visibility = Visibility.Visible;
            }
            else
            {
                MoveTriangleLayer.LeftTriangleArea.Visibility = Visibility.Collapsed;
                MoveTriangleLayer.RightTriangleArea.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Hide navigation triangles on mouse leave.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GlobalGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            MoveTriangleLayer.LeftTriangleArea.Visibility = Visibility.Collapsed;
            MoveTriangleLayer.RightTriangleArea.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Navigate to previous image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftTriangle_Click(object sender, MouseButtonEventArgs e)
        {
            Item.BindingParam.Images.Index--;
            Item.BindingParam.Images.UpdateImage();
        }

        /// <summary>
        /// Navigate to next image.
        /// </summary>
        /// <param name="sender"></param>
        private void RightTriangle_Click(object sender, MouseButtonEventArgs e)
        {
            Item.BindingParam.Images.Index++;
            Item.BindingParam.Images.UpdateImage();
        }


        #endregion
    }
}

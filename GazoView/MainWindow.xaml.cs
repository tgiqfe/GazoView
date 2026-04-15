using GazoView.Lib.Functions;
using GazoView.Lib.Panel;
using System.Windows;
using System.Windows.Input;

namespace GazoView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

        /// <summary>
        /// Key down event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
                case Key.Left:
                    Item.BindingParam.Images.Index--;
                    Item.BindingParam.Images.ViewImage();
                    break;
                case Key.Right:
                    Item.BindingParam.Images.Index++;
                    Item.BindingParam.Images.ViewImage();
                    break;
                case Key.Home:
                    Item.BindingParam.Images.Index = 0;
                    Item.BindingParam.Images.ViewImage();
                    break;
                case Key.End:
                    Item.BindingParam.Images.Index = Item.BindingParam.Images.Length - 1;
                    Item.BindingParam.Images.ViewImage();
                    break;
                case Key.T:
                    //  Switch trimming mode.
                    MoveTriangleLayer.LeftTriangleArea.Visibility = Visibility.Collapsed;
                    MoveTriangleLayer.RightTriangleArea.Visibility = Visibility.Collapsed;
                    ImageFunction.SwitchTrimmingMode();
                    break;
            }
        }

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
            Item.BindingParam.Images.ViewImage();
        }

        /// <summary>
        /// Navigate to next image.
        /// </summary>
        /// <param name="sender"></param>
        private void RightTriangle_Click(object sender, MouseButtonEventArgs e)
        {
            Item.BindingParam.Images.Index++;
            Item.BindingParam.Images.ViewImage();
        }

        #endregion
    }
}

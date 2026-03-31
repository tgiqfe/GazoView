using GazoView.Lib;
using GazoView.Lib.Functions;
using GazoView.Lib.Panel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private void Init()
        {
            Item.MainWindow = this;
            this.DataContext = Item.BindingParam;

            //  Add event (for drag move).
            EventManager.RegisterClassHandler(
                typeof(AdvancedScrollViewer),
                FrameworkElement.MouseLeftButtonDownEvent,
                new MouseButtonEventHandler((sender, e) => this.DragMove()));
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
            }
        }

        /// <summary>
        /// Mouse wheel event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (SpecialKeyStatus.IsCtrlPressed())
            {

            }
            else
            {
                Item.BindingParam.Images.Index += e.Delta > 0 ? -1 : 1;
                Item.BindingParam.Images.ViewImage();
            }
        }

        /// <summary>
        /// Show navigation triangles on mouse move.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GlobalGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (Item.BindingParam.Images == null || Item.BindingParam.Images.Length <= 1)
                return;

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
        /// <param name="e"></param>
        private void RightTriangle_Click(object sender, MouseButtonEventArgs e)
        {
            Item.BindingParam.Images.Index++;
            Item.BindingParam.Images.ViewImage();
        }
    }
}

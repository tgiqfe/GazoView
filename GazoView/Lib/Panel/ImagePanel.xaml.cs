using GazoView.Lib.Functions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GazoView.Lib.Panel
{
    public partial class ImagePanel : UserControl
    {
        public ImagePanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Mouse wheel event handler for zooming and image switching.
        /// If Ctrl is pressed, it zooms the image.
        /// Otherwise, it switches to the next or previous image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (SpecialKeyStatus.IsCtrlPressed())
            {
                Item.BindingParam.ScaleRate.ZoomImage(MainImage, ScrollViewer, e);
            }
            else
            {
                if (Item.BindingParam.RenameBox.IsVisible) return;
                if (Item.BindingParam.DeleteMessage.IsVisible) return;
                Item.BindingParam.Images.Index += e.Delta > 0 ? -1 : 1;
                Item.BindingParam.Images.UpdateImage();
            }
        }

        #region Drag move in scaling mode.

        private Point _startPoint;

        private void ScrollViewer_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Item.BindingParam.ScaleRate.IsScalingMode)
            {
                e.Handled = true;
                _startPoint = e.GetPosition(ScrollViewer);
                Item.MainWindow.Cursor = Cursors.ScrollAll;
            }
        }

        private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (Item.BindingParam.ScaleRate.IsScalingMode && e.RightButton == MouseButtonState.Pressed)
            {
                Point currentPoint = e.GetPosition(ScrollViewer);
                double offsetX = currentPoint.X - _startPoint.X;
                double offsetY = currentPoint.Y - _startPoint.Y;
                ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset - offsetX);
                ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - offsetY);
                _startPoint = currentPoint;
            }
        }

        private void ScrollViewer_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Item.MainWindow.Cursor = Cursors.Arrow;
        }

        #endregion
    }
}

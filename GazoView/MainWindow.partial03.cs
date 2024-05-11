using GazoView.Lib.Conf;
using GazoView.Lib.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GazoView
{
    public partial class MainWindow : Window
    {
        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (SpecialKeyStatus.IsCtrlPressed())
            {
                Item.BindingParam.State.ScalingMode = true;

                //  Ctrlキーが押されている場合、拡大/縮小
                ZoomImage(e);
            }
            else
            {
                //  次の画像/前の画像を表示
                ChangeImage(e.Delta > 0 ? -1 : 1);
            }
        }

        private void MainImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Item.BindingParam.Images.ViewWidth = e.NewSize.Width;
            Item.BindingParam.Images.ViewHeight = e.NewSize.Height;
        }

        #region Move right drag

        private Point _startPoint;
        private Point _startPosition;

        /// <summary>
        /// 拡縮モードで、右クリックドラッグで画像を移動(開始)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Item.BindingParam.State.ScalingMode)
            {
                e.Handled = true;
                _startPoint = e.GetPosition(ScrollViewer);
                _startPosition = new Point(ScrollViewer.HorizontalOffset, ScrollViewer.VerticalOffset);
                ScrollViewer.Cursor = Cursors.ScrollAll;
            }
        }

        /// <summary>
        /// 拡縮モードで、右クリックドラッグで画像を移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if(Item.BindingParam.State.ScalingMode&& e.RightButton == MouseButtonState.Pressed)
            {
                var point = e.GetPosition(ScrollViewer);
                ScrollViewer.ScrollToHorizontalOffset(_startPosition.X - (point.X - _startPoint.X));
                ScrollViewer.ScrollToVerticalOffset(_startPosition.Y - (point.Y - _startPoint.Y));
            }
        }

        /// <summary>
        /// 拡縮モードで、右クリックドラッグで画像を移動(終了)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ScrollViewer.Cursor = Cursors.Arrow;
        }

        #endregion
    }
}

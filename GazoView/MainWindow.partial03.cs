using GazoView.Lib;
using GazoView.Lib.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GazoView
{
    /// <summary>
    /// マウスイベント関連
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// ホイール操作
        ///   Ctrl押し ⇒ 拡大縮小
        ///   Shift押し ⇒ 
        ///   同時押し無し ⇒ 上方向でひとつ前の画像、下方向で次の画像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (SpecialKeyDown.IsCtrlPressed())
            {
                if (!Item.BindingParam.State.ScalingMode)
                {
                    SwitchScalingMode(true);
                }

                if (e.Delta > 0)
                    Item.BindingParam.Images.Current.TickIndex++;
                else
                    Item.BindingParam.Images.Current.TickIndex--;

                var scale = Item.BindingParam.Images.Current.Scale;
                if (scale == 1)
                {
                    MainCanvas.Width = double.NaN;
                    MainCanvas.Height = double.NaN;
                }
                else
                {
                    MainCanvas.Width = MainBase.ActualWidth * scale;
                    MainCanvas.Height = (MainBase.ActualHeight - SystemParameters.WindowCaptionHeight) * scale;
                }
                LabelBar.Content = $"Width: {MainCanvas.Width} Height: {MainCanvas.Height}";



                /*
                //  拡縮したとき、マウスポインタの位置を中心にする 
                Point mousePoint = e.GetPosition(ScrollViewer);
                double x_barOffset = (ScrollViewer.HorizontalOffset + mousePoint.X) * scale - mousePoint.X;
                double y_barOffset = (ScrollViewer.VerticalOffset + mousePoint.Y) * scale - mousePoint.Y;
                ScrollViewer.ScrollToHorizontalOffset(x_barOffset);
                ScrollViewer.ScrollToVerticalOffset(y_barOffset);
                */

            }
            else if (SpecialKeyDown.IsShiftPressed())
            {

            }
            else
            {
                if (e.Delta > 0)
                {
                    KeyEvent_PressLeft();
                }
                else
                {
                    KeyEvent_PressRight();
                }
            }
        }

        private void MainImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*
            if (Item.BindingParam.State.ScalingMode)
            {
                MainCanvas.Width = e.NewSize.Width * 0.9;
                MainCanvas.Height = e.NewSize.Height * 0.9;
            }
            */
        }


        private Point StartPoint;
        private Point StartPosition;


        /// <summary>
        /// 拡縮モードで、右クリックドラッグで画像移動(開始)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Item.BindingParam.State.ScalingMode)
            {
                e.Handled = true;
                StartPoint = e.GetPosition(ScrollViewer);
                StartPosition = new Point(ScrollViewer.HorizontalOffset, ScrollViewer.VerticalOffset);
                ScrollViewer.Cursor = Cursors.ScrollAll;
            }
        }

        /// <summary>
        /// 拡縮モードで、右クリックドラッグで画像移動(移動中)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (Item.BindingParam.State.ScalingMode && e.RightButton == MouseButtonState.Pressed)
            {
                //e.Handled = true;
                Point point = e.GetPosition(ScrollViewer);
                ScrollViewer.ScrollToHorizontalOffset(StartPosition.X - (point.X - StartPoint.X));
                ScrollViewer.ScrollToVerticalOffset(StartPosition.Y - (point.Y - StartPoint.Y));
            }
            else
            {
                ScrollViewer.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// 拡縮モードで、右クリックドラッグで画像移動(終了)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ScrollViewer.Cursor = Cursors.Arrow;
        }
    }
}

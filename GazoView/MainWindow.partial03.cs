using GazoView.Conf;
using GazoView.Lib.Function;
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
        /// <summary>
        /// ホイール操作
        ///   ホイール: 一つ前の画像/次の画像
        /// 　Ctrl+ホイール：拡大縮小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (SpecialKeyStatus.IsCtrlPressed())
            {
                //  画像拡大縮小
                Item.BindingParam.State.ScalingMode = true;

                if (e.Delta > 0)
                {
                    //  拡大
                    if (Item.BindingParam.Images.ScaleRate.IsMax) return;
                    Item.BindingParam.Images.ScaleRate.Index++;
                }
                else
                {
                    //  縮小
                    if (Item.BindingParam.Images.ScaleRate.IsMin) return;
                    Item.BindingParam.Images.ScaleRate.Index--;
                }

                var scale = Item.BindingParam.Images.ScaleRate.Scale;
                if (scale == 1)
                {
                    Item.BindingParam.State.ScalingMode = false;
                    MainImage.Width = ScrollViewer.ActualWidth;
                    MainImage.Height = ScrollViewer.ActualHeight;
                }
                else
                {
                    if (scale > 1)
                    {
                        //  拡縮前のマウスポインタ、スクロール位置を取得
                        Point mousePoint = e.GetPosition(ScrollViewer);
                        double viewX = ScrollViewer.HorizontalOffset;
                        double viewY = ScrollViewer.VerticalOffset;

                        //  拡縮
                        MainImage.Width = this.ActualWidth * scale;
                        MainImage.Height = (this.ActualHeight - SystemParameters.WindowCaptionHeight) * scale;

                        //  スクロール位置を調整
                        var relateScale = scale / Item.BindingParam.Images.ScaleRate.PreviewScale;
                        ScrollViewer.ScrollToHorizontalOffset((mousePoint.X + viewX) * relateScale - mousePoint.X);
                        ScrollViewer.ScrollToVerticalOffset((mousePoint.Y + viewY) * relateScale - mousePoint.Y);
                    }
                    else
                    {
                        MainImage.Width = this.ActualWidth * scale;
                        MainImage.Height = (this.ActualHeight - SystemParameters.WindowCaptionHeight) * scale;
                    }
                }

                //  画像拡大率 300% 以上で、NearestNeighborを有効
                SwitchNearestNeighbor(Item.BindingParam.Images.ImageScalePercent >= 3);
            }
            else if (SpecialKeyStatus.IsShiftPressed())
            {
            }
            else
            {
                //  1つ前の画像/次の画像
                ChangeImage(e.Delta > 0 ? -1 : 1);
            }
        }

        /// <summary>
        /// 画像の表示上のサイズ変更時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Item.BindingParam.Trimming.Scale = e.NewSize.Width / Item.BindingParam.Images.Current.Source.Width;
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
            if (Item.BindingParam.State.ScalingMode && e.RightButton == MouseButtonState.Pressed)
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

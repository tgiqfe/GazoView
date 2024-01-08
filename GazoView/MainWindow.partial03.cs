﻿using GazoView.Conf;
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
                if (!Item.BindingParam.State.ScalingMode) SwitchScalingMode(true);

                if (e.Delta > 0)
                {
                    if (Item.BindingParam.Images.IsMaxScale) return;
                    Item.BindingParam.Images.TickIndex++;
                }
                else
                {
                    if (Item.BindingParam.Images.IsMinScale) return;
                    Item.BindingParam.Images.TickIndex--;
                }

                var scale = Item.BindingParam.Images.Scale;
                if (scale == 1)
                {
                    /*
                    MainCanvas.Width = double.NaN;
                    MainCanvas.Height = double.NaN;
                    */
                    SwitchScalingMode(false);
                }
                else
                {
                    if (scale > 1)
                    {
                        //  拡縮前のマウスポインタ、スクロール位置を取得
                        Point mousePoint = e.GetPosition(ScrollViewer);
                        (double viewX, double viewY) = (ScrollViewer.HorizontalOffset, ScrollViewer.VerticalOffset);

                        //  拡縮
                        MainCanvas.Width = MainBase.ActualWidth * scale;
                        MainCanvas.Height = (MainBase.ActualHeight - SystemParameters.WindowCaptionHeight) * scale;

                        //  スクロール位置を調整
                        var relateScale = scale / Item.BindingParam.Images.PreviewScale;
                        ScrollViewer.ScrollToHorizontalOffset((viewX + mousePoint.X) * relateScale - mousePoint.X);
                        ScrollViewer.ScrollToVerticalOffset((viewY + mousePoint.Y) * relateScale - mousePoint.Y);
                    }
                    else
                    {
                        //  拡縮
                        MainCanvas.Width = MainBase.ActualWidth * scale;
                        MainCanvas.Height = (MainBase.ActualHeight - SystemParameters.WindowCaptionHeight) * scale;
                    }
                }
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

        /// <summary>
        /// MainImageの表示サイズ変更時イベント
        /// BindingParamに表示サイズを送る
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Item.BindingParam.Images.ViewWidth = Math.Round(e.NewSize.Width, 2);
            Item.BindingParam.Images.ViewHeight = Math.Round(e.NewSize.Height, 2);
        }

        #region Move right drag from ScalingMode.

        private Point _startPoint;
        private Point _startPosition;

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
                _startPoint = e.GetPosition(ScrollViewer);
                _startPosition = new Point(ScrollViewer.HorizontalOffset, ScrollViewer.VerticalOffset);
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
                ScrollViewer.ScrollToHorizontalOffset(_startPosition.X - (point.X - _startPoint.X));
                ScrollViewer.ScrollToVerticalOffset(_startPosition.Y - (point.Y - _startPoint.Y));
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

        #endregion
    }
}

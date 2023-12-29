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
    /// <summary>
    /// マウスイベント関連
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// メインウィンドウ(背景部分)でドラッグ移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// 画像上でドラッグ移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// ホイール操作
        ///   同時押し無し ⇒ 上方向でひとつ前の画像、下方向で次の画像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (SpecialKeyDown.IsCtrlPressed())
            {

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

        private void MainCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (SpecialKeyDown.IsCtrlPressed())
            {

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




    }
}

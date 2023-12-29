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
        ///   Ctrl押し ⇒ 拡大縮小
        ///   Shift押し ⇒ 
        ///   同時押し無し ⇒ 上方向でひとつ前の画像、下方向で次の画像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (SpecialKeyDown.IsCtrlPressed())
            {
                if (!Item.BindingParam.State.ScalingMode)
                {
                    SwitchScalingMode(true);
                }

                if (e.Delta > 0)
                {
                    //Item.BindingParam.ImageSizeRate.Value += 0.1;



                }
                else
                {
                    //Item.BindingParam.ImageSizeRate.Value -= 0.1;

                    /*
                    var rate = MainImage.ActualHeight / Item.BindingParam.Images.Current.Height;

                    string text = string.Format("{0} - {1}", 
                        Item.BindingParam.Images.Current.Width, Item.BindingParam.Images.Current.Height);

                    MessageBox.Show(text);
                    */

                    /*
                    double scale = 1;
                    MainCanvas.Width = 1254;
                    MainCanvas.Height = 1770;
                    
                    MainImage.Width = 1254;
                    MainImage.Height = 1770;

                    Matrix matrix = new();
                    matrix.Scale(1, 1);
                    MainCanvas.RenderTransform = new MatrixTransform(matrix);
                    //Point mousePoint = e.GetPosition(ScrollViewer);
                    */


                    (var width, var height, var rate) = ScalingRate.GetScalingRate(MainBase.ActualWidth, MainBase.ActualHeight, -1);
                    MainCanvas.Width = width;
                    MainCanvas.Height = height;
                    MainImage.Width = width;
                    MainImage.Height = height;
                    ScrollViewer.Width = width;
                    ScrollViewer.Height = height;

                    MessageBox.Show(string.Format("{0} - {1}", width, height));
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





        private void MainImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*
            if (Item.BindingParam.State.ScalingMode)
            {
                MainCanvas.Width = e.NewSize.Width * 1;
                MainCanvas.Height = e.NewSize.Height * 1;
            }
            */
        }

    }
}

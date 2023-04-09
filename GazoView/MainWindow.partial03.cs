using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GazoView
{
    /// <summary>
    /// マウスイベント関連を記述
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// ウィンドウ全体でドラッグ可能にする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Item.BindingParam.State.TrimmingMode)
            {
                return;
            }
            this.DragMove();
        }

        /// <summary>
        /// マウスホイール上下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainBase_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down)
            {
                if (!Item.BindingParam.State.ScalingMode)
                {
                    ToggleScalingMode(true);
                }

                if (e.Delta > 0)
                {
                    //  ホイール上方向で、拡大率を一段階上げる
                    Item.BindingParam.ImageSizeRate.Index++;
                }
                else
                {
                    //  ホイール下方向で、拡大率を一段階下げる
                    Item.BindingParam.ImageSizeRate.Index--;
                }

                double scale = Item.BindingParam.ImageSizeRate.Value / Item.BindingParam.ImageSizeRate.PrevValue;
                MainCanvas.Width *= scale;
                MainCanvas.Height *= scale;

                Matrix matrix = new();
                matrix.Scale(Item.BindingParam.ImageSizeRate.Value, Item.BindingParam.ImageSizeRate.Value);
                MainCanvas.RenderTransform = new MatrixTransform(matrix);

                Point mousePoint = e.GetPosition(ScrollViewer);
                double x_barOffset = (ScrollViewer.HorizontalOffset + mousePoint.X) * scale - mousePoint.X;
                ScrollViewer.ScrollToHorizontalOffset(x_barOffset);
                double y_barOffset = (ScrollViewer.VerticalOffset + mousePoint.Y) * scale - mousePoint.Y;
                ScrollViewer.ScrollToVerticalOffset(y_barOffset);

                Item.BindingParam.ImageSizeRate.PrevValue = Item.BindingParam.ImageSizeRate.Value;
            }
            else
            {
                if (e.Delta > 0)
                {
                    //  ホイール上方向でひとつ前の画像へ
                    Item.BindingParam.Images.Index--;
                }
                else if (e.Delta < 0)
                {
                    //  ホイール下方向で一つ後の画像へ
                    Item.BindingParam.Images.Index++;
                }
            }
        }

        /// <summary>
        /// Ctrl+ホイールで画像サイズ変更後処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainCanvas.Width = e.NewSize.Width * Item.BindingParam.ImageSizeRate.Value;
            MainCanvas.Height = e.NewSize.Height * Item.BindingParam.ImageSizeRate.Value;
        }
    }
}

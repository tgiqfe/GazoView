using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using GazoView.Config;
using System.Windows.Threading;
using System.Collections.Specialized;
using System.Windows.Shapes;

namespace GazoView
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 拡縮モードのON/OFF切り替え
        /// </summary>
        /// <param name="toScaling"></param>
        private void ChangeScalingMode(bool toScaling)
        {
            if (toScaling)
            {
                Item.Data.State.ScalingMode = true;
                scalingModeText.Text = "拡縮モード： ON ";
                BindingOperations.ClearBinding(mainImage, Image.WidthProperty);
                BindingOperations.ClearBinding(mainImage, Image.HeightProperty);
            }
            else
            {
                Item.Data.State.ScalingMode = false;
                scalingModeText.Text = "拡縮モード： OFF ";
                mainImage.SetBinding(
                    Image.WidthProperty,
                    new Binding("ActualWidth") { ElementName = "canvas" });
                mainImage.SetBinding(
                    Image.HeightProperty,
                    new Binding("ActualHeight") { ElementName = "canvas" });

                rate.Value = 100;
                Matrix matrix = new Matrix();
                matrix.Scale(1, 1);
                matrixTransform.Matrix = matrix;
                UpdateImage(moveCount: 0);
            }
        }

        /// <summary>
        /// トリミングモードのON/OFFの切り替え
        /// </summary>
        /// <param name="toTrimming"></param>
        private void ChangeTrimmingMode(bool toTrimming)
        {
            if (toTrimming)
            {
                Item.Data.State.TrimmingMode = true;
                trimmingModeText.Text = "トリミングモード： ON ";
                gridRow1.Height = new GridLength(30);
                trimmingModeSetting.Visibility = Visibility.Visible;
                rectangle_left.Visibility = Visibility.Visible;
                rectangle_top.Visibility = Visibility.Visible;
                rectangle_right.Visibility = Visibility.Visible;
                rectangle_bottom.Visibility = Visibility.Visible;
                lineV_left.Visibility = Visibility.Visible;
                lineV_right.Visibility = Visibility.Visible;
                lineH_top.Visibility = Visibility.Visible;
                lineH_bottom.Visibility = Visibility.Visible;
                RenderOptions.SetBitmapScalingMode(mainImage, BitmapScalingMode.NearestNeighbor);
            }
            else
            {
                Item.Data.State.TrimmingMode = false;
                trimmingModeText.Text = "トリミングモード： OFF ";
                gridRow1.Height = new GridLength(0);
                trimmingModeSetting.Visibility = Visibility.Collapsed;
                rectangle_left.Visibility = Visibility.Collapsed;
                rectangle_top.Visibility = Visibility.Collapsed;
                rectangle_right.Visibility = Visibility.Collapsed;
                rectangle_bottom.Visibility = Visibility.Collapsed;
                lineV_left.Visibility = Visibility.Collapsed;
                lineV_right.Visibility = Visibility.Collapsed;
                lineH_top.Visibility = Visibility.Collapsed;
                lineH_bottom.Visibility = Visibility.Collapsed;
                RenderOptions.SetBitmapScalingMode(mainImage, BitmapScalingMode.Fant);
            }
        }

        /// <summary>
        /// ウィンドウ全体でドラッグ可能にする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!Item.Data.State.TrimmingMode)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// マウスホイール上下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down)
            {
                //  拡縮モードON
                ChangeScalingMode(true);

                int index = rate.Ticks.IndexOf(rate.Value);
                if (0 < e.Delta)
                {
                    index += 1;
                }
                else
                {
                    index -= 1;
                }

                if (index <= 0 || rate.Ticks.Count <= index)
                {
                    //  最小倍率/最大倍率の範囲外の場合、拡縮モードOFF
                    //  この挙動は無しにするけれど、必要になればコメントを外す
                    //  ChangeScalingMode(false);
                }
                else
                {
                    double scale = rate.Ticks[index] / rate.Value;
                    rate.Value = rate.Ticks[index];

                    canvas.Height *= scale;
                    canvas.Width *= scale;

                    Matrix matrix = new Matrix();
                    matrix.Scale(rate.Value * 0.01, rate.Value * 0.01);
                    matrixTransform.Matrix = matrix;

                    Point mousePoint = e.GetPosition(scrollViewer);
                    double x_barOffset = (scrollViewer.HorizontalOffset + mousePoint.X) * scale - mousePoint.X;
                    scrollViewer.ScrollToHorizontalOffset(x_barOffset);

                    double y_barOffset = (scrollViewer.VerticalOffset + mousePoint.Y) * scale - mousePoint.Y;
                    scrollViewer.ScrollToVerticalOffset(y_barOffset);
                }
            }
            else
            {
                if (e.Delta > 0)
                {
                    //  ホイール上方向で一つ前の画像へ
                    UpdateImage(moveCount: -1);
                }
                else if (e.Delta < 0)
                {
                    //  ホイール下方向で一つ後の画像へ
                    UpdateImage(moveCount: 1);
                }
            }
        }

        /// <summary>
        /// Ctrl+ホイールでの画像サイズ変更時、Canvasのサイズも同時に変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double scale = rate.Value / 100;

            canvas.Height = e.NewSize.Height * scale;
            canvas.Width = e.NewSize.Width * scale;
        }

        /// <summary>
        /// マウスオーバーイベント
        /// イメージタイトルを表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            imageTitle.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// マウスリーブイベント
        /// イメージタイトルを非表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            imageTitle.Visibility = Visibility.Hidden;
        }

        #region Drag and Drop

        /// <summary>
        /// ファイルをDragOver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropFiles)
            {
                e.Effects = File.Exists(dropFiles[0]) ?
                    DragDropEffects.Copy : DragDropEffects.None;
                e.Handled = true;

                mainImage.Opacity = 0.6;
                this.Background = Item.Data.Theme.DoragOverBackground;
            }
        }

        /// <summary>
        /// DragOverしていたファイルをLeave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewDragLeave(object sender, DragEventArgs e)
        {
            mainImage.Opacity = 1.0;
            this.Background = Item.Data.Theme.WindowBackground;
        }

        /// <summary>
        /// ファイルをDropIn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewDrop(object sender, DragEventArgs e)
        {
            mainImage.Opacity = 1.0;
            this.Background = Item.Data.Theme.WindowBackground;
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropFiles)
            {
                Item.ImageStore.SetItems(dropFiles);
                UpdateImage(moveCount: 0);
            }
        }
        #endregion

        #region Right drag move

        Point _startPoint;
        Point _startPosition;

        private void scrollViewer_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Item.Data.State.ScalingMode)
            {
                _startPoint = e.GetPosition(scrollViewer);
                _startPosition = new Point(scrollViewer.HorizontalOffset, scrollViewer.VerticalOffset);

                scrollViewer.Cursor = Cursors.ScrollAll;
                e.Handled = true;
            }
        }

        /// <summary>
        /// 拡縮モードの場合のみ、ScrollViewer上で右クリック移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Item.Data.State.ScalingMode && e.RightButton == MouseButtonState.Pressed)
            {
                Point _currentPoint = e.GetPosition(scrollViewer);
                double moveX = _startPosition.X - (_currentPoint.X - _startPoint.X);
                double moveY = _startPosition.Y - (_currentPoint.Y - _startPoint.Y);

                scrollViewer.ScrollToHorizontalOffset(moveX);
                scrollViewer.ScrollToVerticalOffset(moveY);
            }
            else
            {
                scrollViewer.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// トリミングモードの場合のみ、Canvas上でLine移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (Item.Data.State.TrimmingMode && e.LeftButton == MouseButtonState.Pressed)
            {
                Point point = e.GetPosition(canvas);
                double newX = Math.Floor(point.X);
                if (newX < -2) { newX = -2; }
                if (newX > mainImage.ActualWidth + 2) { newX = mainImage.ActualWidth + 2; }

                double newY = Math.Floor(point.Y);
                if (newY < -2) { newY = -2; }
                if (newY > mainImage.ActualHeight + 2) { newY = mainImage.ActualHeight + 2; }

                if (_dragLine == DragLine.None)
                {
                    if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
                        (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down)
                    {
                        _dragLine =
                            Math.Abs(newY - lineH_top.Y1) < Math.Abs(newY - lineH_bottom.Y1) ?
                                DragLine.VerticalTop : DragLine.VerticalBottom;
                    }
                    else
                    {
                        _dragLine =
                            Math.Abs(newX - lineV_left.X1) < Math.Abs(newX - lineV_right.X1) ?
                                DragLine.HorizontalLeft : DragLine.HorizontalRight;
                    }
                }

                switch (_dragLine)
                {
                    case DragLine.VerticalTop:
                        if (newY <= lineH_bottom.Y1)
                        {
                            lineH_top.Y1 = newY;
                        }
                        break;
                    case DragLine.VerticalBottom:
                        if (lineH_top.Y1 <= newY)
                        {
                            lineH_bottom.Y1 = newY;
                        }
                        break;
                    case DragLine.HorizontalLeft:
                        if (newX <= lineV_right.X1)
                        {
                            lineV_left.X1 = newX;
                        }
                        break;
                    case DragLine.HorizontalRight:
                        if (lineV_left.X1 <= newX)
                        {
                            lineV_right.X1 = newX;
                        }
                        break;
                }
            }
            else
            {
                _dragLine = DragLine.None;
            }
        }

        private void scrollViewer_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.Cursor = Cursors.Arrow;
        }

        #endregion

        #region Trimming

        /// <summary>
        /// ドラッグ中の対象
        /// </summary>
        DragLine _dragLine = DragLine.None;

        /// <summary>
        /// Lineおドラッグ開始時に対象を固定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lineX_X_back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Line line)
            {
                switch (line.Name)
                {
                    case "lineV_left": _dragLine = DragLine.HorizontalLeft; break;
                    case "lineV_right": _dragLine = DragLine.HorizontalRight; break;
                    case "lineH_top": _dragLine = DragLine.VerticalTop; break;
                    case "lineH_bottom": _dragLine = DragLine.VerticalBottom; break;
                }
            }
        }

        #endregion
    }
}

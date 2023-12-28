using System;
using System.Collections.Generic;
using System.IO;
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
                //  拡縮モードで拡大/縮小率変更
                if (!Item.BindingParam.State.ScalingMode)
                {
                    ToggleScalingMode(true);
                }

                if (e.Delta > 0)
                {
                    //  ホイール上方向で、拡大率を一段階上げる
                    Item.BindingParam.ImageSizeRate.Index++;
                }
                else if (e.Delta < 0)
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
            else if ((Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) == KeyStates.Down)
            {
                if (Item.BindingParam.State.TransparentMode)
                {
                    //  透明モード時
                    if (e.Delta > 0)
                    {
                        //  ホイール上方向で、不透明度を一段階上げる
                        Item.BindingParam.WindowOpacity.Index++;
                    }
                    else if (e.Delta < 0)
                    {
                        //  ホイール下方向で、不透明度を一段階下げる
                        Item.BindingParam.WindowOpacity.Index--;
                    }
                }
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
            if (Item.BindingParam.State.ScalingMode)
            {
                MainCanvas.Width = e.NewSize.Width * Item.BindingParam.ImageSizeRate.Value;
                MainCanvas.Height = e.NewSize.Height * Item.BindingParam.ImageSizeRate.Value;
            }
        }


        /// <summary>
        /// 拡縮モードで、右クリックで画像移動 (開始)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Item.BindingParam.State.ScalingMode)
            {
                e.Handled = true;
                Item.StartPoint_RightButtonMove = e.GetPosition(ScrollViewer);
                Item.StartPosition_RightButtonMove = new Point(ScrollViewer.HorizontalOffset, ScrollViewer.VerticalOffset);
                ScrollViewer.Cursor = Cursors.ScrollAll;
            }
        }

        /// <summary>
        /// 拡縮モードで、右クリックで画像移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (Item.BindingParam.State.ScalingMode && e.RightButton == MouseButtonState.Pressed)
            {
                Point _currentPoint = e.GetPosition(ScrollViewer);
                double moveX = Item.StartPosition_RightButtonMove.X - (_currentPoint.X - Item.StartPoint_RightButtonMove.X);
                double moveY = Item.StartPosition_RightButtonMove.Y - (_currentPoint.Y - Item.StartPoint_RightButtonMove.Y);
                ScrollViewer.ScrollToHorizontalOffset(moveX);
                ScrollViewer.ScrollToVerticalOffset(moveY);
            }
            else
            {
                ScrollViewer.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// 拡縮モードで、右クリックで画像移動 (終了)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ScrollViewer.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// ファイルをDragOver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainBase_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropFiles)
            {
                if (File.Exists(dropFiles[0]) || Directory.Exists(dropFiles[0]))
                {
                    e.Effects = DragDropEffects.Copy;
                    MainImage.Opacity = 0.6;
                    this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3187F0"));
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// DragOverしていたファイルをLeave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainBase_PreviewDragLeave(object sender, DragEventArgs e)
        {
            MainImage.Opacity = 1;
            this.Background = Brushes.DimGray;
        }

        /// <summary>
        /// ファイルをDropIn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainBase_PreviewDrop(object sender, DragEventArgs e)
        {
            MainImage.Opacity = 1;
            this.Background = Brushes.DimGray;
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropFiles)
            {
                Item.BindingParam.Images.UpdateFileList(dropFiles);
            }
        }
    }
}

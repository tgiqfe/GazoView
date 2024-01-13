using GazoView.Conf;
using GazoView.Lib.Function;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GazoView
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 画像を変更
        /// direction ⇒ -1:前の画像, 1:次の画像
        /// </summary>
        /// <param name="direction"></param>
        private void ChangeImage(int direction)
        {
            Item.BindingParam.Images.Index += direction;

            Item.BindingParam.Trimming.Scale =
                MainImage.ActualWidth / Item.BindingParam.Images.Current.Source.Width;

            //  画像拡大率 300% 以上で、NearestNeighborを有効
            SwitchNearestNeighbor(Item.BindingParam.Images.ImageScalePercent >= 3);
        }

        #region File drag and drop

        /// <summary>
        /// ファイルDragOver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
            if (e.Data.GetData(DataFormats.FileDrop) is string[] targets)
            {
                if (File.Exists(targets[0]) || Directory.Exists(targets[0]))
                {
                    e.Effects = DragDropEffects.Copy;
                    MainImage.Opacity = 0.6;
                    this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3395FF"));
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        /// <summary>
        /// ファイルをDragLeave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewDragLeave(object sender, DragEventArgs e)
        {
            MainImage.Opacity = 1;
            this.Background = Brushes.DimGray;
        }

        /// <summary>
        /// ファイルをDropIn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewDrop(object sender, DragEventArgs e)
        {
            MainImage.Opacity = 1;
            this.Background = Brushes.DimGray;
            if(e.Data.GetData(DataFormats.FileDrop) is string[] targets)
            {
                Item.BindingParam.Images.LoadFiles(targets);
            }
        }

        #endregion

        /// <summary>
        /// トリミング実行
        /// </summary>
        private void StartTrimming()
        {
            string output = FileAction.CreateSafePath(Item.BindingParam.Images.Current.FilePath);

            var ret = MessageBox.Show($"Trim.\r\n[ {output} ]",
                Item.ProcessName,
                MessageBoxButton.YesNo,
                MessageBoxImage.Information,
                MessageBoxResult.Yes);
            if (ret != MessageBoxResult.Yes) return;

            ImageTrimming.Cut(
                Item.BindingParam.Images.Current.Source,
                Item.BindingParam.Images.Current.FilePath,
                output,
                Item.BindingParam.Images.Current.FileExtension,
                Item.BindingParam.Trimming.Left,
                Item.BindingParam.Trimming.Top,
                Item.BindingParam.Trimming.Right - Item.BindingParam.Trimming.Left,
                Item.BindingParam.Trimming.Bottom - Item.BindingParam.Trimming.Top);
        }
    }
}

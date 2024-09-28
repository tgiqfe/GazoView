using GazoView.Lib.Conf;
using GazoView.Lib.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace GazoView
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 画像を変更
        /// direction => 1: 次の画像, -1: 前の画像, 5: 5つ次の画像, -5: 5つ前の画像
        /// </summary>
        /// <param name="direction"></param>
        private void ChangeImage(int direction)
        {
            if (!Item.BindingParam.State.IsTrimmingSizeChanging)
            {
                Item.BindingParam.Images.Index += direction;
                Item.BindingParam.Trimming.Scale =
                    MainImage.ActualWidth / Item.BindingParam.Images.Current.Source.Width;

                //  画像拡大率 300% 以上で、NearestNeighborを有効
                SwitchNearestNeighbor(Item.BindingParam.Images.ImageScalePercent >= 3);
            }
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
            if (e.Data.GetData(DataFormats.FileDrop) is string[] targets)
            {
                Item.BindingParam.Images.LoadFiles(targets);
            }
        }

        #endregion

        #region Delete and Restore

        /// <summary>
        /// 現在選択中のファイルを削除
        /// </summary>
        private void DeleteFile()
        {
            if (Item.BindingParam.Images.FileList.Count > 0)
            {
                var ret = MessageBox.Show($"Delete?\n{Item.BindingParam.Images.Current.FileName}",
                    Item.ProcessName,
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.None,
                    MessageBoxResult.OK);
                if (ret == MessageBoxResult.OK)
                {
                    Item.DeletedStore ??= new();
                    Item.DeletedStore.CopyToDeletedStore(Item.BindingParam.Images.Current.FilePath);
                    Item.BindingParam.Images.Delete();
                }
            }
        }

        /// <summary>
        /// 削除したファイルを復元
        /// </summary>
        private void RestoreFile()
        {
            if (Item.DeletedStore?.DeletedList?.Count > 0)
            {
                var ret = MessageBox.Show($"Restore?\n{Item.DeletedStore.RestorableFileName}",
                    Item.ProcessName,
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.None,
                    MessageBoxResult.OK);
                if (ret == MessageBoxResult.OK)
                {
                    Item.DeletedStore.RestoreFromDeletedStore(Item.BindingParam.Images.Current.Parent);
                    Item.BindingParam.Images.ReloadFiles(Item.DeletedStore.RestoredFilePath);
                }
            }
        }

        /// <summary>
        /// 現在のファイルをクリップボードにコピー
        /// </summary>
        private void CopyImageFile(bool isText = false)
        {
            if (Item.BindingParam.Images.FileList.Count > 0)
            {
                if (isText)
                {
                    string text = Item.BindingParam.Images.Current.FilePath;
                    Clipboard.SetText(text);
                }
                else
                {
                    var targets = new System.Collections.Specialized.StringCollection();
                    targets.Add(Item.BindingParam.Images.Current.FilePath);
                    Clipboard.SetFileDropList(targets);
                }
            }
        }

        #endregion

        private void StartTrimming()
        {
            if (Item.BindingParam.State.TrimmingMode)
            {
                string output = FilePaths.Deduplicate(Item.BindingParam.Images.Current.FilePath);

                var ret = MessageBox.Show($"Trim.\r\n[ {output} ]",
                    Item.ProcessName,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information,
                    MessageBoxResult.Yes);
                if (ret != MessageBoxResult.Yes) return;

                var (left, top, width, height) = (
                    Item.BindingParam.Trimming.Left,
                    Item.BindingParam.Trimming.Top,
                    Item.BindingParam.Trimming.Right - Item.BindingParam.Trimming.Left,
                    Item.BindingParam.Trimming.Bottom - Item.BindingParam.Trimming.Top);
                ImageTrimming.Cut(
                    Item.BindingParam.Images.Current.Source,
                    Item.BindingParam.Images.Current.FilePath,
                    output,
                    Item.BindingParam.Images.Current.FileExtension,
                    left, top, width, height);

                string lastHistroyText = $"{top},{top + height},{left},{left + width}";
                if (lastHistroyText != Item.BindingParam.Setting.Histories[0])
                {
                    Item.BindingParam.Setting.Histories.Insert(0, lastHistroyText);
                }
                if (Item.BindingParam.Setting.Histories.Count > Item.BindingParam.Setting.MaxHistory)
                {
                    Item.BindingParam.Setting.Histories.RemoveAt(Item.BindingParam.Setting.Histories.Count - 1);
                }
            }
        }
    }
}

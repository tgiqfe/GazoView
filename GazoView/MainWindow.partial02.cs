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
        /// ファイルをDragOver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropFiles)
            {
                if (File.Exists(dropFiles[0]) || Directory.Exists(dropFiles[0]))
                {
                    e.Effects = DragDropEffects.Copy;
                    MainImage.Opacity = 0.6;
                    this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3395FF"));
                }
                else
                {
                    MessageBox.Show("aaa");
                    e.Effects = DragDropEffects.None;
                }
            }
        }

        /// <summary>
        /// ファイルをDragLeav
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewDragLeave(object sender, DragEventArgs e)
        {
            MainImage.Opacity = 1.0;
            this.Background = Brushes.DimGray;
        }

        /// <summary>
        /// ファイルをDropIn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewDrop(object sender, DragEventArgs e)
        {
            MainImage.Opacity = 1.0;
            this.Background = Brushes.DimGray;
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropFiles)
            {
                Item.BindingParam.Images.SetFileList(dropFiles);
            }
        }
    }
}

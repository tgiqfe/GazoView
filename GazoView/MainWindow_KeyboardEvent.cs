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
using System.IO;
using System.Collections.Specialized;
using GazoView.Functions;

namespace GazoView
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// キー押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    //  アプリケーション終了
                    Application.Current.Shutdown();
                    break;
                case Key.Left:
                    //  一つ前の画像へ
                    e.Handled = true;
                    UpdateImage(-1);
                    break;
                case Key.Right:
                    //  一つ後ろの画像へ
                    e.Handled = true;
                    UpdateImage(1);
                    break;
                case Key.Delete:
                    //  ファイルを削除
                    bool isShiftDown =
                        (Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) == KeyStates.Down ||
                        (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) == KeyStates.Down;

                    MessageBoxResult delRet = MessageBox.Show(
                        "File Delete OK?\r\n" + Item.ImageStore.CurrentPath,
                        "GazoView",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question,
                        MessageBoxResult.Yes);
                    if (delRet == MessageBoxResult.Yes)
                    {
                        DeleteImage(forceDelete: isShiftDown);
                    }
                    break;
                case Key.F:
                    //  外部アプリケーションで画像を開く
                    AlternateApplication.Execute(Item.ImageStore.CurrentPath);
                    break;
                case Key.D:
                    //  画像情報を表示
                    if (imageInfoPanel.Visibility == Visibility.Hidden)
                    {
                        imageInfoPanel.Visibility = Visibility.Visible;
                        imageInfoPanel2.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        imageInfoPanel.Visibility = Visibility.Hidden;
                        imageInfoPanel2.Visibility = Visibility.Hidden;
                    }
                    UpdateImageInfo();
                    break;
                case Key.C:
                    //  画像ファイルをコピー
                    if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
                        (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down)
                    {
                        var clipFiles = new StringCollection();
                        clipFiles.Add(Item.ImageStore.CurrentPath);
                        Clipboard.SetFileDropList(clipFiles);
                    }
                    break;
                case Key.O:
                    //  画像ファイルの場所を開く
                    if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
                        (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down)
                    {
                        using (var proc = new System.Diagnostics.Process())
                        {
                            proc.StartInfo.FileName = "explorer.exe";
                            proc.StartInfo.Arguments = "/select, \"" + Item.ImageStore.CurrentPath + "\"";
                            proc.Start();
                        }
                    }
                    break;
                case Key.R:
                    //  拡縮モード
                    if (Item.Data.State.ScalingMode)
                    {
                        ChangeScalingMode(false);
                        ChangeTrimmingMode(false);
                    }
                    else if (mainImage.Source != null)
                    {
                        ChangeScalingMode(true);
                    }
                    break;
                case Key.T:
                    //  トリミングモード
                    if (Item.Data.State.TrimmingMode)
                    {
                        ChangeTrimmingMode(false);
                    }
                    else if (mainImage.Source != null)
                    {
                        ChangeScalingMode(true);
                        ChangeTrimmingMode(true);
                    }
                    break;
                case Key.G:
                    //  トリミング開始
                    if (Item.Data.State.TrimmingMode)
                    {
                        string trimmedPath = ImageTrimming.Cut((BitmapSource)Item.ImageStore.CurrentImageSource,
                            Item.ImageStore.CurrentPath,
                            lineV_left.X1 + 2,
                            lineH_top.Y1 + 2,
                            lineV_right.X1 - lineV_left.X1 - 4,
                            lineH_bottom.Y1 - lineH_top.Y1 - 4);
                        MessageBox.Show("Trim.\r\n" + trimmedPath,
                            "GazoView",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    break;
            }
        }

        /// <summary>
        /// トリム用設定エリアで数字以外を入力禁止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trimmingTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out int tempInt))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// トリム用設定エリアで上下キー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trimmingTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox box)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        box.Text = (int.Parse(box.Text) + 1).ToString();
                        break;
                    case Key.Down:
                        box.Text = (int.Parse(box.Text) - 1).ToString();
                        break;
                }
            }
        }
    }
}

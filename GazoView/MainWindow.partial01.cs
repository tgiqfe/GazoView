using GazoView.Conf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GazoView.Lib;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

namespace GazoView
{
    public partial class MainWindow : Window
    {
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    //  アプリケーション終了
                    Application.Current.Shutdown();
                    break;
                case Key.R:
                    //  拡縮モードの無効化
                    SwitchScalingMode(false);
                    break;
                case Key.T:
                    SwitchTrimmingMode();
                    break;
                case Key.G:
                    //  トリミング実行
                    StartTrimming();
                    break;
                case Key.D:
                case Key.Tab:
                    //  Infoパネルの表示/非表示
                    if (Item.BindingParam.State.IsTrimmingSizeChanging) break;
                    ChangeInfoPanel();
                    break;
                case Key.N:
                    //  最近傍法の有効/無効を切り替え
                    SwitchNearestNeighbor();
                    break;
                case Key.Left:
                    //  1つ前の画像を表示
                    if (Item.BindingParam.State.IsTrimmingSizeChanging) break;
                    ChangeImage(-1);
                    break;
                case Key.Right:
                    //  1つ後の画像を表示
                    if (Item.BindingParam.State.IsTrimmingSizeChanging) break;
                    ChangeImage(1);
                    break;
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    //Debug.WriteLine("Ctrl");
                    break;
            }
        }
    }
}

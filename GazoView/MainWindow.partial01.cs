﻿using GazoView.Lib.Conf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GazoView
{
    public partial class MainWindow : Window
    {
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    //  アプリケーションを終了
                    Application.Current.Shutdown();
                    break;
                case Key.Left:
                    //  1つ前の画像を表示
                    ChangeImage(-1);
                    break;
                case Key.Right:
                    //  1つ後の画像を表示
                    ChangeImage(1);
                    break;
                case Key.Home:
                    //  最初の画像を表示
                    ChangeImage(Item.BindingParam.Images.Length);
                    break;
                case Key.End:
                    //  最後の画像を表示
                    ChangeImage(-1 * Item.BindingParam.Images.Length);
                    break;
                case Key.D:
                case Key.Tab:
                    //  Infoパネルの表示/非表示
                    ChangeInfoPanel();
                    break;
                case Key.T:
                    SwitchTrimmingMode();
                    break;
            }
        }
    }
}

﻿using GazoView.Lib.Conf;
using GazoView.Lib.Functions;
using GazoView.Lib.Other;
using System.Windows;
using System.Windows.Controls;
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
                    //  トリミングモードの切り替え
                    SwitchTrimmingMode();
                    break;
                case Key.G:
                    //  トリミング実行
                    StartTrimming();
                    break;
                case Key.Delete:
                    //  画像を削除
                    DeleteFile();
                    break;
                case Key.Z:
                    //  削除した画像を復元
                    if (SpecialKeyStatus.IsCtrlPressed())
                    {
                        RestoreFile();
                    }
                    break;
                case Key.C:
                    //  画像をコピー (Ctrl+Cでファイルをコピー。Ctrl+Shift+Cでファイルパスをコピー)
                    if (SpecialKeyStatus.IsCtrlPressed())
                    {
                        if (SpecialKeyStatus.IsShiftPressed())
                        {
                            CopyImageFile(true);
                        }
                        else
                        {
                            CopyImageFile();
                        }
                    }
                    break;
                case Key.O:
                    //  画像のフォルダーを開く
                    FolderWindow.Open(
                        Item.BindingParam.Images.Current.Parent,
                        Item.BindingParam.Images.Current.FileName);
                    break;
                case Key.OemBackslash:
                    ToggleStarFile();
                    break;
            }
        }
    }
}

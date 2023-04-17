using GazoView.Lib.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GazoView
{
    /// <summary>
    /// キーイベント関連を記述
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    KeyEvent_PressEsc(); break;
                case Key.Delete:
                    KeyEvent_PressDelete(); break;
                case Key.Insert:
                    KeyEvent_PressInsert(); break;
                case Key.Tab:
                case Key.D:
                    KeyEvent_PressD(); break;
                case Key.T:
                    KeyEvent_PressT(); break;
                case Key.G:
                    KeyEvent_PressG(); break;
                case Key.R:
                    KeyEvent_PressR(); break;
                case Key.E:
                    KeyEvent_PressE(); break;
                case Key.C:
                    KeyEvent_PressC(); break;
                case Key.O:
                    KeyEvent_PressO(); break;
                case Key.Left:
                    KeyEvent_PressLeft(); break;
                case Key.Right:
                    KeyEvent_PressRight(); break;
                case Key.Up:
                    KeyEvent_PressUp(); break;
                case Key.Down:
                    KeyEvent_PressDown(); break;
                case Key.D1:
                    KeyEvent_Press1(); break;
                case Key.D2:
                    KeyEvent_Press2(); break;
                case Key.D3:
                    KeyEvent_Press3(); break;
                case Key.D4:
                    KeyEvent_Press4(); break;
                case Key.D5:
                    KeyEvent_Press5(); break;
                case Key.D6:
                    KeyEvent_Press6(); break;
                case Key.D7:
                    KeyEvent_Press7(); break;
                case Key.D8:
                    KeyEvent_Press8(); break;
                case Key.D9:
                    KeyEvent_Press9(); break;
                case Key.D0:
                    KeyEvent_Press0(); break;
            }

            //  Alt同時押しの場合
            switch (e.SystemKey)
            {
                case Key.C:
                    KeyEvent_PressC(withAlt: true); break;
            }
        }

        /// <summary>
        /// キー押下時イベント: Esc
        /// アプリケーション終了
        /// </summary>
        private void KeyEvent_PressEsc()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// キー押下時イベント: Delete
        /// 現在開いている画像ファイルを削除
        /// </summary>
        private void KeyEvent_PressDelete()
        {
            bool isShiftDown =
                (Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) == KeyStates.Down;
            var result = MessageBox.Show(
                "File Dleete OK?\r\n[ " + Item.BindingParam.Images.Current.FileName + " ]",
                Item.ProcessName,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                Item.BindingParam.Images.DeleteCurrentFile(force: isShiftDown);
            }
        }

        /// <summary>
        /// キー押下時イベント: Insert
        /// </summary>
        private void KeyEvent_PressInsert()
        {
            var result = MessageBox.Show(
                "File Move OK?\r\n[ " + Item.BindingParam.Images.Current.FileName + " ]",
                Item.ProcessName,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                Item.BindingParam.Images.MoveCurrentFile();
            }
        }

        /// <summary>
        /// キー押下時イベント: D
        /// 画像情報を表示
        /// </summary>
        private void KeyEvent_PressD()
        {
            InfoImage1.Visibility = InfoImage1.Visibility == Visibility.Visible ?
                Visibility.Hidden :
                Visibility.Visible;
            InfoImage2.Visibility = InfoImage2.Visibility == Visibility.Visible ?
                Visibility.Hidden :
                Visibility.Visible;
        }

        /// <summary>
        /// キー押下時イベント: T
        /// トリミングモード
        /// </summary>
        private void KeyEvent_PressT()
        {
            if (!Item.BindingParam.State.TransparentMode)
            {
                ToggleTrimmingMode();
                ToggleScalingMode(Item.BindingParam.State.TrimmingMode);
            }
        }

        /// <summary>
        /// キー押下時イベント: G
        /// トリミング実行
        /// </summary>
        private void KeyEvent_PressG()
        {
            if (Item.BindingParam.State.TrimmingMode)
            {
                var trim = new ImageTrimming(
                    Item.BindingParam.Images.ImageSource,
                    Item.BindingParam.Images.Current.FilePath,
                    Item.BindingParam.Setting.Trimming.Left,
                    Item.BindingParam.Setting.Trimming.Top,
                    Item.BindingParam.Setting.Trimming.Width,
                    Item.BindingParam.Setting.Trimming.Height);
                trim.Cut();
                MessageBox.Show("Trim.\r\n" +
                    "[ " + trim.OutputPath + " ]",
                    Item.ProcessName,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// キー押下イベント: R
        /// 拡縮モード
        /// </summary>
        private void KeyEvent_PressR()
        {
            if (!Item.BindingParam.State.TransparentMode)
            {
                ToggleScalingMode();
                if (!Item.BindingParam.State.ScalingMode && Item.BindingParam.State.TrimmingMode)
                {
                    ToggleTrimmingMode(false);
                }
            }
        }

        /// <summary>
        /// キー押下時イベント: E
        /// 透明モード
        /// </summary>
        private void KeyEvent_PressE()
        {
            if (!Item.BindingParam.State.TrimmingMode)
            {
                ToggleTransparentMode();
            }
        }

        /// <summary>
        /// キー押下時イベント: C
        /// Ctrl+C ⇒ クリップボードにファイルパスをコピー
        /// Alt+C ⇒ クリップボードに画像をコピー
        /// </summary>
        private void KeyEvent_PressC(bool withAlt = false)
        {
            if (withAlt)
            {
                var bitmap = Item.BindingParam.Images.ImageSource as BitmapImage;
                Clipboard.SetImage(bitmap);
            }
            else if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down)
            {
                Clipboard.SetText(Item.BindingParam.Images.Current.FilePath);
            }
        }

        /// <summary>
        /// キー押下時イベント: O
        /// エクスプローラで開く
        /// </summary>
        private void KeyEvent_PressO()
        {
            FileAction.OpenExplorerForFile(Item.BindingParam.Images.Current.FilePath);
        }

        /// <summary>
        /// キー押下時イベント: Left
        /// 前の画像を表示
        /// </summary>
        private void KeyEvent_PressLeft()
        {
            Item.BindingParam.Images.Index--;
        }

        /// <summary>
        /// キー押下時イベント: Right
        /// 次の画像を表示
        /// </summary>
        private void KeyEvent_PressRight()
        {
            Item.BindingParam.Images.Index++;
        }

        /// <summary>
        /// キー押下時イベント: Up
        /// 透明化モード時⇒Opacity値上昇
        /// </summary>
        private void KeyEvent_PressUp()
        {
            if (Item.BindingParam.State.TransparentMode &&
                ((Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) == KeyStates.Down))
            {
                Item.BindingParam.WindowOpacity.Index++;
            }
        }

        /// <summary>
        /// キー押下時イベント: Down
        /// /// 透明化モード時⇒Opacity値下降
        /// </summary>
        private void KeyEvent_PressDown()
        {
            if (Item.BindingParam.State.TransparentMode &&
                ((Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) == KeyStates.Down))
            {
                Item.BindingParam.WindowOpacity.Index--;
            }
        }

        /// <summary>
        /// キー押下時イベント: 1
        /// 外部アプリケーションで開く
        /// </summary>
        private void KeyEvent_Press1()
        {
            FileAction.ExecuteAltenateApp(Item.BindingParam.Setting.AltenateApp1, Item.BindingParam.Images.Current.FilePath);
        }

        /// <summary>
        /// キー押下時イベント: 2
        /// 外部アプリケーションで開く
        /// </summary>
        private void KeyEvent_Press2()
        {
            FileAction.ExecuteAltenateApp(Item.BindingParam.Setting.AltenateApp2, Item.BindingParam.Images.Current.FilePath);
        }

        /// <summary>
        /// キー押下時イベント: 3
        /// 外部アプリケーションで開く
        /// </summary>
        private void KeyEvent_Press3()
        {
            FileAction.ExecuteAltenateApp(Item.BindingParam.Setting.AltenateApp3, Item.BindingParam.Images.Current.FilePath);
        }

        /// <summary>
        /// キー押下時イベント: 4
        /// 外部アプリケーションで開く
        /// </summary>
        private void KeyEvent_Press4()
        {
            FileAction.ExecuteAltenateApp(Item.BindingParam.Setting.AltenateApp4, Item.BindingParam.Images.Current.FilePath);
        }

        /// <summary>
        /// キー押下時イベント: 5
        /// 外部アプリケーションで開く
        /// </summary>
        private void KeyEvent_Press5()
        {
            FileAction.ExecuteAltenateApp(Item.BindingParam.Setting.AltenateApp5, Item.BindingParam.Images.Current.FilePath);
        }

        /// <summary>
        /// キー押下時イベント: 6
        /// 外部アプリケーションで開く
        /// </summary>
        private void KeyEvent_Press6()
        {
            FileAction.ExecuteAltenateApp(Item.BindingParam.Setting.AltenateApp6, Item.BindingParam.Images.Current.FilePath);
        }

        /// <summary>
        /// キー押下時イベント: 7
        /// 外部アプリケーションで開く
        /// </summary>
        private void KeyEvent_Press7()
        {
            FileAction.ExecuteAltenateApp(Item.BindingParam.Setting.AltenateApp7, Item.BindingParam.Images.Current.FilePath);
        }

        /// <summary>
        /// キー押下時イベント: 8
        /// 外部アプリケーションで開く
        /// </summary>
        private void KeyEvent_Press8()
        {
            FileAction.ExecuteAltenateApp(Item.BindingParam.Setting.AltenateApp8, Item.BindingParam.Images.Current.FilePath);
        }

        /// <summary>
        /// キー押下時イベント: 9
        /// 外部アプリケーションで開く
        /// </summary>
        private void KeyEvent_Press9()
        {
            FileAction.ExecuteAltenateApp(Item.BindingParam.Setting.AltenateApp9, Item.BindingParam.Images.Current.FilePath);
        }

        /// <summary>
        /// キー押下時イベント: 0
        /// 外部アプリケーションで開く
        /// </summary>
        private void KeyEvent_Press0()
        {
            FileAction.ExecuteAltenateApp(Item.BindingParam.Setting.AltenateApp0, Item.BindingParam.Images.Current.FilePath);
        }
    }
}

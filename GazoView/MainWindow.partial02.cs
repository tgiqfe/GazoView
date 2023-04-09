using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace GazoView
{
    /// <summary>
    /// キーイベント関連を記述
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            switch (e.Key)
            {
                case Key.Escape:
                    KeyEvent_PressEsc(); break;
                case Key.D:
                    KeyEvent_PressD(); break;
                case Key.T:
                    KeyEvent_PressT(); break;
                case Key.R:
                    KeyEvent_PressR(); break;
                case Key.E:
                    KeyEvent_PressE(); break;
                case Key.Left:
                    KeyEvent_PressLeft(); break;
                case Key.Right:
                    KeyEvent_PressRight(); break;
                case Key.Up:
                    KeyEvent_PressUp(); break;
                case Key.Down:
                    KeyEvent_PressDown(); break;
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
                ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down))
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
                ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down))
            {
                Item.BindingParam.WindowOpacity.Index--;
            }
        }
    }
}

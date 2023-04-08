using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            switch (e.Key)
            {
                case Key.Escape:
                    KeyEvent_PressEsc(); break;
                case Key.D:
                    KeyEvent_PressD(); break;
                case Key.T:
                    KeyEvent_PressT(); break;

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

        private void KeyEvent_PressT()
        {
            InfoTrimmingBar.Visibility = InfoTrimmingBar.Visibility == Visibility.Visible ?
                Visibility.Hidden :
                Visibility.Visible;
        }
    }
}

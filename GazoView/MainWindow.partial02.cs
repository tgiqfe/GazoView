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
                case Key.Escape: KeyEvent_PressEsc(); break;

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
    }
}

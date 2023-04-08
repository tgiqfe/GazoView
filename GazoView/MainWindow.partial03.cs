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
    /// マウスイベント関連を記述
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// ウィンドウ全体でドラッグ可能にする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Item.BindingParam.State.TrimmingMode)
            {
                return;
            }
            this.DragMove();
        }
    }
}

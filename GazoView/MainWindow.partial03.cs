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
        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            InfoTitleBar.Visibility = Visibility.Visible;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            InfoTitleBar.Visibility = Visibility.Hidden;
        }
    }
}

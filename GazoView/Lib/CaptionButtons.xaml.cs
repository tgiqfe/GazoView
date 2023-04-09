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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GazoView.Lib
{
    /// <summary>
    /// CaptionButtons.xaml の相互作用ロジック
    /// </summary>
    public partial class CaptionButtons : UserControl
    {
        public CaptionButtons()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ウィンドウ最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Item.MainBase.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// ウィンドウ最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Item.MainBase.WindowState == WindowState.Maximized)
            {
                Item.MainBase.WindowState = WindowState.Normal;
                MaximizeButton.Content = "1";
            }
            else
            {
                Item.MainBase.WindowState = WindowState.Maximized;
                MaximizeButton.Content = "2";
            }
        }

        /// <summary>
        /// 終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

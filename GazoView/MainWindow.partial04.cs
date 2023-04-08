using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace GazoView
{
    /// <summary>
    /// 特定のタイミングで実行するアクション
    /// </summary>
    public partial class MainWindow : Window
    {
        private void ChangeScalingMode(bool toScaling)
        {
            Item.BindingParam.Setting.ScalingMode = toScaling;
            if (toScaling)
            {

            }
            else
            {
                MainImage.SetBinding(
                    Image.WidthProperty,
                    new Binding("ActualWidth") { ElementName = "MainCanvas" });
                MainImage.SetBinding(
                    Image.HeightProperty,
                    new Binding("ActualHeight") { ElementName = "MainCanvas" });
            }
        }

        /// <summary>
        /// ウィンドウサイズ変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            


            


        }
    }
}

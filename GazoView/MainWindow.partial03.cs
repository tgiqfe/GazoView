using GazoView.Lib.Functions;
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
        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if(SpecialKeyStatus.IsCtrlPressed())
            {
                //  Ctrlキーが押されている場合
                if(e.Delta > 0)
                {
                    //  拡大
                    //ZoomImage(1.1);
                }
                else
                {
                    //  縮小
                    //ZoomImage(0.9);
                }
            }
            else
            {
                //  次の画像/前の画像を表示
                ChangeImage(e.Delta > 0 ? -1 : 1);
            }
        }
    }
}

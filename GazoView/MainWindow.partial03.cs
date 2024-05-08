using GazoView.Lib.Conf;
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
                Item.BindingParam.State.ScalingMode = true;

                //  Ctrlキーが押されている場合、拡大/縮小
                ZoomImage(e);
            }
            else
            {
                //  次の画像/前の画像を表示
                ChangeImage(e.Delta > 0 ? -1 : 1);
            }
        }

        private void MainImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Item.BindingParam.Images.ViewWidth = e.NewSize.Width;
            Item.BindingParam.Images.ViewHeight = e.NewSize.Height;
        }
    }
}

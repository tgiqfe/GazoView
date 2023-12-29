using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GazoView
{
    public partial class MainWindow : Window
    {


        private void SwitchScalingMode(bool? toScaling = null)
        {
            Item.BindingParam.State.ScalingMode =
                toScaling ?? !Item.BindingParam.State.ScalingMode;

            if (Item.BindingParam.State.ScalingMode)
            {
                //  拡縮モードを有効化
                //BindingOperations.ClearBinding(MainImage, Image.WidthProperty);
                //BindingOperations.ClearBinding(MainImage, Image.HeightProperty);
                //  Item.BindingParam.ImageSizeRate.Enabled = true;
            }
            else
            {
                //  拡縮モードを無効化
                /*
                MainImage.SetBinding(
                    Image.WidthProperty,
                    new Binding("ActualWidth") { ElementName = "MainCanvas" });
                MainImage.SetBinding(
                    Image.HeightProperty,
                    new Binding("ActualHeight") { ElementName = "MainCanvas" });
                */

                

                //  Item.BindingParam.ImageSizeRate.Enabled = false;


                //  拡縮モードから戻したときに、Setting内のWidthとHeightに相違がある場合の処理


            }


        }


    }
}

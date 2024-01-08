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

        /// <summary>
        /// 拡縮モードのON/OFF
        /// </summary>
        /// <param name="toScaling"></param>
        private void SwitchScalingMode(bool? toScaling = null)
        {
            Item.BindingParam.State.ScalingMode =
                toScaling ?? !Item.BindingParam.State.ScalingMode;

            if (Item.BindingParam.State.ScalingMode)
            {
                //  拡縮モードを有効化
                var scale = Item.BindingParam.Images.Scale;
                MainCanvas.Width = MainBase.ActualWidth * scale;
                MainCanvas.Height = (MainBase.ActualHeight - SystemParameters.WindowCaptionHeight) * scale;
            }
            else
            {
                //  拡縮モードを無効化
                MainCanvas.Width = double.NaN;
                MainCanvas.Height = double.NaN;
            }
        }


    }
}

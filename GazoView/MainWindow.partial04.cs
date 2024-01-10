using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using GazoView.Conf;

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
            }
            else
            {
                //  拡縮モードを無効化
                Item.BindingParam.Images.SetDefaultScale();
                MainCanvas.Width = double.NaN;
                MainCanvas.Height = double.NaN;
            }
        }

        /// <summary>
        /// トリミングモードのON/OFF
        /// </summary>
        /// <param name="toTrimming"></param>
        private void SwitchTrimmingMode(bool? toTrimming = null)
        {
            Item.BindingParam.State.TrimmingMode =
                toTrimming ?? !Item.BindingParam.State.TrimmingMode;


            string text = Item.BindingParam.Trimming.Bottom.ToString();
            double takasa = MainCanvas.ActualHeight - Item.BindingParam.Trimming.Bottom;
            LabelBar.Content = $"Height: {MainCanvas.ActualHeight} Bottom: {text} Takasa: {takasa}";

            if (Item.BindingParam.State.TrimmingMode)
            {
                //  トリミングモードを有効化
                MainRow1.Height = new GridLength(30);
            }
            else
            {
                //  トリミングモードを無効化
                MainRow1.Height = new GridLength(0);
            }
        }   

        /// <summary>
        /// Bitmapスケーリングの切り替え
        /// Fant ⇔ NearestNeighbor
        /// </summary>
        /// <param name="toNearestNeighbor"></param>
        private void BitmapScalingMode(bool? toNearestNeighbor = null)
        {
            Item.BindingParam.State.NearestNeighbor =
                toNearestNeighbor ?? !Item.BindingParam.State.NearestNeighbor;

            if (Item.BindingParam.State.NearestNeighbor)
            {
                //  BitmapScalingModeをNearestNeighborに変更
                RenderOptions.SetBitmapScalingMode(MainImage, System.Windows.Media.BitmapScalingMode.NearestNeighbor);
            }
            else
            {
                //  BitmapScalingModeをFantに変更
                RenderOptions.SetBitmapScalingMode(MainImage, System.Windows.Media.BitmapScalingMode.Fant);
            }
        }

        /// <summary>
        /// Infoパネルの表示/非表示
        /// </summary>
        /// <param name="toShow"></param>
        private void SwitchShowInfoPanel(bool? toShow = null)
        {
            Item.BindingParam.State.ShowInfoPanel =
                toShow ?? !Item.BindingParam.State.ShowInfoPanel;
        }
    }
}

using GazoView.Conf;
using GazoView.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GazoView
{
    public partial class MainWindow : Window
    {
        private void SwitchScalingMode(bool? toEnable = null)
        {
            Item.BindingParam.State.ScalingMode =
                toEnable ?? !Item.BindingParam.State.ScalingMode;

            if (Item.BindingParam.State.ScalingMode)
            {
                //  拡縮モードの有効化
            }
            else
            {
                //  拡縮モードの無効化
                Item.BindingParam.Images.ScaleRate.Init();
                MainImage.Width = ScrollViewer.ActualWidth;
                MainImage.Height = ScrollViewer.ActualHeight;
            }
        }

        /// <summary>
        /// Infoパネルの表示/非表示
        /// </summary>
        private void ChangeInfoPanel()
        {
            Item.BindingParam.State.InfoPanelIndex++;
            if (Item.BindingParam.State.InfoPanelIndex > 2)
            {
                Item.BindingParam.State.InfoPanelIndex = 0;
            }

            Item.InfoPanel1 ??= new InfoPanel1();

            switch (Item.BindingParam.State.InfoPanelIndex)
            {
                case 0:
                    GlobalGrid.Children.Remove(Item.InfoPanel1);
                    Column0.Width = new GridLength(0);
                    break;
                case 1:
                    Grid.SetColumn(Item.InfoPanel1, 1);
                    GlobalGrid.Children.Add(Item.InfoPanel1);
                    break;
                case 2:
                    GlobalGrid.Children.Remove(Item.InfoPanel1);
                    Grid.SetColumn(Item.InfoPanel1, 0);
                    GlobalGrid.Children.Add(Item.InfoPanel1);
                    Column0.Width = new GridLength(300);
                    break;
            }
        }

        /// <summary>
        /// 最近傍法の有効/無効を切り替え
        /// </summary>
        /// <param name="toEnable"></param>
        private void SwitchNearestNeighbor(bool? toEnable = null)
        {
            Item.BindingParam.State.NearestNeighbor = 
                toEnable ?? !Item.BindingParam.State.NearestNeighbor;


            if (Item.BindingParam.State.NearestNeighbor)
            {
                RenderOptions.SetBitmapScalingMode(MainImage, BitmapScalingMode.NearestNeighbor);
            }
            else
            {
                RenderOptions.SetBitmapScalingMode(MainImage, BitmapScalingMode.Fant);
            }
        }
    }
}

using GazoView.Lib.Conf;
using GazoView.Lib.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GazoView
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Infoパネルの表示/非表示を切り替える
        /// </summary>
        private void ChangeInfoPanel()
        {
            Item.InfoPanel ??= new();
            Item.BindingParam.State.InfoPanel ^= true;

            if (Item.BindingParam.State.InfoPanel)
            {
                if (SpecialKeyStatus.IsShiftPressed())
                {
                    //  Shiftを押しながら
                    Grid.SetColumn(Item.InfoPanel, 0);
                    GlobalGrid.Children.Add(Item.InfoPanel);
                    Column0.Width = new GridLength(300);
                }
                else
                {
                    //  Shiftを押さないで
                    Grid.SetColumn(Item.InfoPanel, 1);
                    GlobalGrid.Children.Add(Item.InfoPanel);
                    Item.InfoPanel.HorizontalAlignment = HorizontalAlignment.Left;
                }
            }
            else
            {
                GlobalGrid.Children.Remove(Item.InfoPanel);
                Column0.Width = new GridLength(0);
            }
        }

        private void ZoomImage(MouseWheelEventArgs e)
        {
            int direction = e.Delta > 0 ? 1 : -1;
            if ((Item.BindingParam.Images.ScaleRate.IsMax && direction > 0) ||
                (Item.BindingParam.Images.ScaleRate.IsMin && direction < 0))
            {
                return;
            }
            Item.BindingParam.Images.ScaleRate.Index += direction;

            var scale = Item.BindingParam.Images.ScaleRate.Scale;
            if (scale == 1)
            {
                Item.BindingParam.State.ScalingMode = false;
                MainImage.Width = ScrollViewer.ActualWidth;
                MainImage.Height = ScrollViewer.ActualHeight;
            }
            else
            {
                if (scale > 1)
                {
                    //  拡縮前のマウスポインタ、スクロール位置を取得
                    Point mousePoint = e.GetPosition(ScrollViewer);
                    double viewX = ScrollViewer.HorizontalOffset;
                    double viewY = ScrollViewer.VerticalOffset;

                    //  拡縮
                    MainImage.Width = this.ActualWidth * scale;
                    MainImage.Height = (this.ActualHeight - SystemParameters.WindowCaptionHeight) * scale;

                    //  スクロール位置を調整
                    var relateScale = scale / Item.BindingParam.Images.ScaleRate.PreviewScale;
                    ScrollViewer.ScrollToHorizontalOffset((mousePoint.X + viewX) * relateScale - mousePoint.X);
                    ScrollViewer.ScrollToVerticalOffset((mousePoint.Y + viewY) * relateScale - mousePoint.Y);
                }
                else
                {
                    MainImage.Width = this.ActualWidth * scale;
                    MainImage.Height = (this.ActualHeight - SystemParameters.WindowCaptionHeight) * scale;
                }
            }

            //  画像拡大率 300% 以上で、NearestNeighborを有効
            SwitchNearestNeighbor(Item.BindingParam.Images.ImageScalePercent >= 3);
        }

        /// <summary>
        /// 画像拡大率、表示拡大率を100%にリセット
        /// </summary>
        private void ResetZoomScale()
        {
            Item.BindingParam.Images.ScaleRate.Index = ScaleRate.DEF_INDEX;
            Item.BindingParam.State.ScalingMode = false;
            MainImage.Width = ScrollViewer.ActualWidth;
            MainImage.Height = ScrollViewer.ActualHeight;
        }

        /// <summary>
        /// トリミングモードに切り替え
        /// 切り替え前の下準備も
        /// </summary>
        /// <param name="toEnable"></param>
        private void SwitchTrimmingMode(bool? toEnable = null)
        {
            if (Item.BindingParam.Setting.Histories == null || Item.BindingParam.Setting.Histories.Count == 0)
            {
                Item.BindingParam.Setting.Histories = new List<string>() { "100,300,100,300" };
            }
            var history = Item.BindingParam.Setting.Histories[0].Split(',').Select(x => int.Parse(x)).ToArray();

            if (Item.BindingParam.Trimming.Top < 0)
            {
                Item.BindingParam.Trimming.Top = history[0];
            }
            if (Item.BindingParam.Trimming.Bottom < 0)
            {
                Item.BindingParam.Trimming.Bottom = history[1];
            }
            if (Item.BindingParam.Trimming.Left < 0)
            {
                Item.BindingParam.Trimming.Left = history[2];
            }
            if (Item.BindingParam.Trimming.Right < 0)
            {
                Item.BindingParam.Trimming.Right = history[3];
            }

            Item.BindingParam.State.TrimmingMode =
                toEnable ?? !Item.BindingParam.State.TrimmingMode;
        }

        /// <summary>
        /// 最近傍法の有効/無効を切り替え
        /// </summary>
        /// <param name="toEnable"></param>
        private void SwitchNearestNeighbor(bool? toEnable = null)
        {
            bool nextState = toEnable ?? !Item.BindingParam.State.NearestNeighbor;
            if (Item.BindingParam.State.NearestNeighbor != nextState)
            {
                Item.BindingParam.State.NearestNeighbor = nextState;
                if (nextState)
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
}

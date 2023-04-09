using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;
using GazoView.Lib.Functions;

namespace GazoView
{
    /// <summary>
    /// 特定のタイミングで実行するアクション
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// トリミングモードの切り替え
        /// </summary>
        /// <param name="toTrimming"></param>
        private void ToggleTrimmingMode(bool? toTrimming = null)
        {
            Item.BindingParam.State.TrimmingMode = toTrimming == null ?
                !Item.BindingParam.State.TrimmingMode :
                (bool)toTrimming;

            if (Item.BindingParam.State.TrimmingMode)
            {
                MainRow1.Height = new GridLength(25);
            }
            else
            {
                MainRow1.Height = new GridLength(0);
            }

        }

        /// <summary>
        /// 拡縮モードの切り替え
        /// </summary>
        /// <param name="toScaling"></param>
        private void ToggleScalingMode(bool? toScaling = null)
        {
            Item.BindingParam.State.ScalingMode = toScaling == null ?
                !Item.BindingParam.State.ScalingMode :
                (bool)toScaling;

            if (Item.BindingParam.State.ScalingMode)
            {
                BindingOperations.ClearBinding(MainImage, Image.WidthProperty);
                BindingOperations.ClearBinding(MainImage, Image.HeightProperty);
                Item.BindingParam.ImageSizeRate.Enabled = true;
            }
            else
            {
                MainImage.SetBinding(
                    Image.WidthProperty,
                    new Binding("ActualWidth") { ElementName = "MainCanvas" });
                MainImage.SetBinding(
                    Image.HeightProperty,
                    new Binding("ActualHeight") { ElementName = "MainCanvas" });
                Item.BindingParam.ImageSizeRate.Enabled = false;

                if (MainCanvas.Width != Item.BindingParam.Setting.Width ||
                    MainCanvas.Height != Item.BindingParam.Setting.Height)
                {
                    Item.BindingParam.ImageSizeRate.Value = 1;
                    MainCanvas.Width = double.NaN;
                    MainCanvas.Height = double.NaN;
                    Matrix matrix = new();
                    matrix.Scale(1, 1);
                    MainCanvas.RenderTransform = new MatrixTransform(matrix);
                }
            }
        }

        /// <summary>
        /// 透明モードの切り替え
        /// </summary>
        /// <param name="toTransparent"></param>
        private void ToggleTransparentMode(bool? toTransparent = null)
        {
            Item.BindingParam.State.TransparentMode = toTransparent == null ?
                !Item.BindingParam.State.TransparentMode :
                (bool)toTransparent;

            if (Item.BindingParam.State.TransparentMode)
            {
                SetBinding(
                    Window.OpacityProperty,
                    new Binding("Value") { Source = Item.BindingParam.WindowOpacity });
                Item.BindingParam.WindowOpacity.Enabled = true;
            }
            else
            {
                BindingOperations.ClearBinding(MainBase, Window.OpacityProperty);
                Item.BindingParam.WindowOpacity.Enabled = false;
            }
        }

        /// <summary>
        /// 最大化/最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainBase_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Minimized:
                    WorkingSet.Shrink();

                    //  その他最小化時の処理をここに

                    break;
                case WindowState.Normal:
                case WindowState.Maximized:

                    //  その他通常化/最大化時の処理をここに

                    break; ;
            }
        }
    }
}

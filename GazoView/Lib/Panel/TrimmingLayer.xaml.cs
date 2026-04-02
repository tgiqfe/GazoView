using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace GazoView.Lib.Panel
{
    public partial class TrimmingLayer : UserControl
    {
        public TrimmingLayer()
        {
            InitializeComponent();
        }

        private void TrimmingLayer_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!Item.IsInitialized) return;
            if (this.IsVisible)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    double viewScale = 1.0;
                    if (this.ActualWidth < Item.BindingParam.Images.Current.Width)
                    {
                        viewScale = this.ActualWidth / Item.BindingParam.Images.Current.Width;
                    }
                    if (this.ActualHeight < Item.BindingParam.Images.Current.Height)
                    {
                        viewScale = Math.Min(viewScale, this.ActualHeight / Item.BindingParam.Images.Current.Height);
                    }

                    double viewWidth = Item.BindingParam.Images.Current.Width * viewScale;
                    double viewHeight = Item.BindingParam.Images.Current.Height * viewScale;
                    int trimmingLayerLeft = 0;
                    int trimmingLayerTop = 0;
                    if (Item.BindingParam.Images.Current.Width > Item.BindingParam.Images.Current.Height)
                    {
                        trimmingLayerTop = (int)((this.ActualHeight - viewHeight) / 2);
                    }
                    else if (Item.BindingParam.Images.Current.Width < Item.BindingParam.Images.Current.Height)
                    {
                        trimmingLayerLeft = (int)((this.ActualWidth - viewWidth) / 2);
                    }

                    //  TrimmingMode to enable event.
                    if (Item.BindingParam.Trimming.Top < 0)
                    {
                        int top = (int)(viewHeight / 3);
                        //int top = 100;
                        Item.BindingParam.Trimming.Top = top;
                        Debug.WriteLine($"Trimming.Top is set to {top}.");
                    }
                    if (Item.BindingParam.Trimming.Bottom < 0 || viewHeight > this.ActualHeight)
                    {
                        int bottom = (int)(viewHeight / 3 * 2);
                        //int bottom = 200;
                        Item.BindingParam.Trimming.Bottom = bottom;
                        Debug.WriteLine($"Trimming.Bottom is set to {bottom}.");
                    }
                    if (Item.BindingParam.Trimming.Left < 0)
                    {
                        int left = (int)(viewWidth / 3);
                        //int left = 100;
                        Item.BindingParam.Trimming.Left = left;
                        Debug.WriteLine($"Trimming.Left is set to {left}.");
                    }
                    if (Item.BindingParam.Trimming.Right < 0 || viewWidth > this.ActualWidth)
                    {
                        int right = (int)(viewWidth / 3 * 2);
                        //int right = 200;
                        Item.BindingParam.Trimming.Right = right;
                        Debug.WriteLine($"Trimming.Right is set to {right}.");
                    }
                }), System.Windows.Threading.DispatcherPriority.Loaded);
            }
            else
            {
                //  TrimmingMode to disable event.
            }
        }
    }
}

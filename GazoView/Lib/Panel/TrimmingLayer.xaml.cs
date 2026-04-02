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
            if (IsVisible)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    //  TrimmingMode to enable event.
                    if (Item.BindingParam.Trimming.Top < 0)
                    {
                        //int top = (int)(this.ActualHeight / 3);
                        int top = 100;
                        Item.BindingParam.Trimming.Top = top;
                        Debug.WriteLine($"Trimming.Top is set to {top}.");
                    }
                    if (Item.BindingParam.Trimming.Bottom < 0 || Item.BindingParam.Trimming.Bottom > this.ActualHeight)
                    {
                        //int bottom = (int)(this.ActualHeight / 3 * 2);
                        int bottom = 200;
                        Item.BindingParam.Trimming.Bottom = bottom;
                        Debug.WriteLine($"Trimming.Bottom is set to {bottom}.");
                    }
                    if (Item.BindingParam.Trimming.Left < 0)
                    {
                        //int left = (int)(this.ActualWidth / 4);
                        int left = 100;
                        Item.BindingParam.Trimming.Left = left;
                        Debug.WriteLine($"Trimming.Left is set to {left}.");
                    }
                    if (Item.BindingParam.Trimming.Right < 0 || Item.BindingParam.Trimming.Right > this.ActualWidth)
                    {
                        //int right = (int)(this.ActualWidth / 4 * 2);
                        int right = 200;
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

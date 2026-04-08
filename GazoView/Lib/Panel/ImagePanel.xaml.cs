using GazoView.Lib.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GazoView.Lib.Panel
{
    public partial class ImagePanel : UserControl
    {
        public ImagePanel()
        {
            InitializeComponent();
            this.DataContext = Item.BindingParam;
        }

        private void UserControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (SpecialKeyStatus.IsCtrlPressed())
            {
                Item.BindingParam.ScaleRate.IsScalingMode = true;
                Item.BindingParam.ScaleRate.ZoomImage(MainImage, ScrollViewer, e);
            }
            else
            {
                Item.BindingParam.Images.Index += e.Delta > 0 ? -1 : 1;
                Item.BindingParam.Images.ViewImage();
            }
        }
    }
}

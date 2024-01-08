﻿using GazoView.Lib;
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

namespace GazoView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = Item.BindingParam;
            Item.Mainbase = this;
            SwitchScalingMode(false);



            
        }


    }
}



/*

後から有効/無効に切り替える必要が出てきたら復旧

MainImage.SetBinding(
    Image.WidthProperty,
    new Binding("ActualWidth") { ElementName = "MainCanvas" });
MainImage.SetBinding(
    Image.HeightProperty,
    new Binding("ActualHeight") { ElementName = "MainCanvas" });
*/
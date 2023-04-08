using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            //  各モードの初期設定
            ChangeScalingMode(toScaling: false);
            //ChangeTriggminMode
            //ChangeTransparentMode


            //  サンプルで画像指定
            using (var fs = new FileStream(@"C:\aaaa.jpg", FileMode.Open, FileAccess.Read))
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.CreateOptions = BitmapCreateOptions.None;
                bmp.StreamSource = fs;
                bmp.EndInit();
                bmp.Freeze();
                
                MainImage.Source = bmp;
            }


        }


    }
}

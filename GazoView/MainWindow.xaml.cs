using GazoView.Conf;
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

            Item.Mainbase = this;
            this.DataContext = Item.BindingParam;

            //  画像拡大率 300% 以上で、NearestNeighborを有効
            SwitchNearestNeighbor(Item.BindingParam.Images.ImageScalePercent >= 3);
        }
    }
}

/*
 * NearestNeighbor変更のタイミング
 * - 最初に画像を開いたとき (MainIndowのInitializeComponent()メソッドの中)
 * - 画像を切り替えたとき (ChangeImage()メソッドの中)
 * - 画像拡大率を変更したとき (Window_PreviewMouseWheel()メソッドの中)
 */
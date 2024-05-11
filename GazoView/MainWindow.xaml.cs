using GazoView.Lib.Conf;
using System.Windows;
using System.Windows.Input;

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

            Item.MainBase = this;
            this.DataContext = Item.BindingParam;
        }


    }
}

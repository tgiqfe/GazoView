using GazoView.Conf;
using System;
using System.Collections.Generic;
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

namespace GazoView.Lib
{
    /// <summary>
    /// InfoPanel2.xaml の相互作用ロジック
    /// </summary>
    public partial class InfoPanel2 : UserControl
    {
        public InfoPanel2()
        {
            InitializeComponent();
            this.DataContext = Item.BindingParam;
        }
    }
}

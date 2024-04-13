using GazoView.Lib.Conf;
using System.Windows.Controls;

namespace GazoView.Lib
{
    /// <summary>
    /// InfoPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class InfoPanel : UserControl
    {
        public InfoPanel()
        {
            InitializeComponent();
            this.DataContext = Item.BindingParam;
        }
    }
}

using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GazoView.Lib.Panel
{
    /// <summary>
    /// ParamInputPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class ParamInputPanel : UserControl
    {
        public ParamInputPanel()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}

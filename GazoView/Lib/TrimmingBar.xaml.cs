using GazoView.Lib.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// TrimmingBar.xaml の相互作用ロジック
    /// </summary>
    public partial class TrimmingBar : UserControl
    {
        public TrimmingBar()
        {
            InitializeComponent();

            this.DataContext = Item.BindingParam;
        }

        private Regex _regex = new Regex(@"[0-9]");

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (Item.BindingParam.MouseMovingInTrimming) { return; }
            if (string.IsNullOrEmpty(tb.Text)) { return; }

            switch (tb.Name)
            {
                case "TextBoxTop":
                    Item.BindingParam.Setting.Trimming.Top = int.Parse(tb.Text);
                    break;
                case "TextBoxBottom":
                    Item.BindingParam.Setting.Trimming.Bottom = int.Parse(tb.Text);
                    break;
                case "TextBoxLeft":
                    Item.BindingParam.Setting.Trimming.Left = int.Parse(tb.Text);
                    break;
                case "TextBoxRight":
                    Item.BindingParam.Setting.Trimming.Right = int.Parse(tb.Text);
                    break;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Item.BindingParam.State.TrimmingMode)
            {
                var trim = (Trimming)((ComboBox)sender).SelectedItem;
                Item.BindingParam.Setting.Trimming.Top = trim.Top;
                Item.BindingParam.Setting.Trimming.Bottom = trim.Bottom;
                Item.BindingParam.Setting.Trimming.Left = trim.Left;
                Item.BindingParam.Setting.Trimming.Right = trim.Right;
            }
        }
    }
}

using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GazoView.Lib.Panel
{
    public partial class TrimInfoPanelInput : UserControl
    {
        public TrimInfoPanelInput()
        {
            InitializeComponent();
        }

        private void TextBox_NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is not TextBox textBox) return;
            if (!int.TryParse(textBox.Text, out int currentValue)) return;

            if (e.Key == Key.Enter)
            {
                switch (textBox.Name)
                {
                    case nameof(TopTextBox):
                        Item.BindingParam.Trimming.Top = currentValue;
                        break;
                    case nameof(BottomTextBox):
                        Item.BindingParam.Trimming.Bottom = currentValue;
                        break;
                    case nameof(LeftTextBox):
                        Item.BindingParam.Trimming.Left = currentValue;
                        break;
                    case nameof(RightTextBox):
                        Item.BindingParam.Trimming.Right = currentValue;
                        break;
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Up || e.Key == Key.Down)
            {
                int delta = e.Key switch
                {
                    Key.Up => 1,
                    Key.Down => -1,
                    _ => 0
                };

                if (delta != 0)
                {
                    int newValue = currentValue + delta;
                    switch (textBox.Name)
                    {
                        case nameof(TopTextBox):
                            Item.BindingParam.Trimming.Top = newValue;
                            break;
                        case nameof(BottomTextBox):
                            Item.BindingParam.Trimming.Bottom = newValue;
                            break;
                        case nameof(LeftTextBox):
                            Item.BindingParam.Trimming.Left = newValue;
                            break;
                        case nameof(RightTextBox):
                            Item.BindingParam.Trimming.Right = newValue;
                            break;
                    }
                    textBox.SelectAll();
                    e.Handled = true;
                }
            }
        }
    }
}

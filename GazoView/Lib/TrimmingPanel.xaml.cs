using GazoView.Conf;
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
    /// TrimmingPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class TrimmingPanel : UserControl
    {
        public TrimmingPanel()
        {
            InitializeComponent();
            this.DataContext = Item.BindingParam;
        }

        #region Numeric only

        private Regex regex_numeric = new Regex("[0-9]");

        /// <summary>
        /// TextBoxでの入力を制限する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !regex_numeric.IsMatch(e.Text);
        }

        /// <summary>
        /// 貼り付けを無効化する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        #endregion
        #region TextBox focus

        /// <summary>
        /// TrimmingPanelの中のTextBoxにフォーカスが当たったとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Item.BindingParam.State.IsTrimmingSizeChanging = true;

            //  選択時に全選択
            var target = (TextBox)sender;
            target.SelectAll();
        }

        /// <summary>
        /// TrimmingPanelの中のTextBoxからフォーカスが外れたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Item.BindingParam.State.IsTrimmingSizeChanging = false;
        }

        #endregion
    }
}

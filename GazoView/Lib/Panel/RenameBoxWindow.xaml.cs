using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GazoView.Lib.Panel
{
    /// <summary>
    /// RenameBoxWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class RenameBoxWindow : Window
    {
        public RenameBoxWindow()
        {
            InitializeComponent();
            this.DataContext = Item.BindingParam;
        }

        private void RenameBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxForFileName.Focus();
            TextBoxForFileName.Select(TextBoxForFileName.Text.Length, 0);
        }

        private void RenameWindowBase_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!TextBoxForFileName.IsFocused)
            {
                TextBoxForFileName.Focus();
                TextBoxForFileName.SelectAll();
            }
        }

        /// <summary>
        /// Key down event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
                case Key.Enter:
                    break;
            }
        }


    }
}

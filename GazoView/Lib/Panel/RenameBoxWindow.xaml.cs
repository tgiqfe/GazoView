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
        }

        private void RenameWindowBase_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!TextBoxForFileName.IsFocused)
            {
                TextBoxForFileName.Focus();
                TextBoxForFileName.SelectAll();
            }
        }
    }
}

using GazoView.Lib.Functions;
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
using System.Windows.Shapes;

namespace GazoView.Lib
{
    /// <summary>
    /// MessageWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MessageWindow : Window
    {
        public bool IsClosed { get; set; }

        public MessageWindow()
        {
            InitializeComponent();
            this.DataContext = Item.BindingParam;
            Button_OK.Focus();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.IsClosed = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Close();
                    break;
                case Key.Left:
                    Button_Cancel.Focus();
                    break;
                case Key.Right:
                    Button_OK.Focus();
                    break;
            }
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            FileFunction.DeleteImageFile(Item.BindingParam.Images);
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GazoView.Lib.Panel
{
    public partial class MessageDialogWindow : Window
    {
        public MessageDialogWindow()
        {
            InitializeComponent();
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button eventObject = e.Source as Button;
            if (eventObject == null)
            {
                this.DragMove();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
                case Key.Enter:
                    ButtonOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
            }
        }
    }
}

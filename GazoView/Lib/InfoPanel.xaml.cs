using GazoView.Lib.Conf;
using MaterialDesignThemes.Wpf;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewCopiedMessage();
        }

        private void ViewCopiedMessage()
        {
            PackIcon packIcon = new()
            {
                Kind = PackIconKind.TextBoxCheck,
                Width = 24,
                Height = 24,
                Foreground = Brushes.White,
                Padding = new Thickness(0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(2, 2, 2, 2),
            };

            Label label = new()
            {
                Content = "Copied!",
                FontSize = 14,
                FontFamily = new FontFamily("Noto Sans JP"),
                Foreground = Brushes.White,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 3, 0, 3),
                Padding = new Thickness(0),
            };

            StackPanel stackPanel = new()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(20, 150, 0, 0),
                Width = 100,
                Height = 28,
                Background = Brushes.DimGray,
            };
            stackPanel.Children.Add(packIcon);
            stackPanel.Children.Add(label);

            InfoPanelGrid.Children.Add(stackPanel);
            Task.Delay(3000).ContinueWith(_ =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    InfoPanelGrid.Children.Remove(stackPanel);
                });
            });
        }
    }
}

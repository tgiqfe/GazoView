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

        #region Copy to clipboard

        private bool _isViewCopiedMessage = false;

        /// <summary>
        /// テキストブロック部分をクリックしたときに、内容をクリップボードにコピー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!_isViewCopiedMessage)
            {
                _isViewCopiedMessage = true;
                var point = e.GetPosition(InfoPanelGrid);
                ViewCopiedMessage(point.X, point.Y);

                var tbox = (TextBlock)sender;
                Clipboard.SetText(tbox.Text);
            }
        }

        /// <summary>
        /// 「Copied!」メッセージを表示する
        /// </summary>
        private void ViewCopiedMessage(double pointX, double pointY)
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
                Margin = new Thickness(pointX, pointY, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
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
                    _isViewCopiedMessage = false;
                });
            });
        }

        #endregion
    }
}

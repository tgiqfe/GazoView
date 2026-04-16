using GazoView.Lib.Panel;
using System.Windows;

namespace GazoView.Lib
{
    public class RenameBox
    {
        private RenameBoxWindow _renameBoxWindow;
        private double _windowShadowSize = -1;
        private double _titleBarSize = -1;

        public bool IsVisible { get; set; }

        public void ShowWindow()
        {
            if (_windowShadowSize < 0)
            {
                _windowShadowSize = (Item.MainWindow.ActualWidth - ((FrameworkElement)Item.MainWindow.Content).ActualWidth) / 2;
            }
            if (_titleBarSize < 0)
            {
                _titleBarSize = (Item.MainWindow.ActualHeight - ((FrameworkElement)Item.MainWindow.Content).ActualHeight) - _windowShadowSize;
            }

            _renameBoxWindow ??= new RenameBoxWindow();
            _renameBoxWindow.Owner = Item.MainWindow;
            _renameBoxWindow.Show();
            _renameBoxWindow.Left = Item.MainWindow.Left + _windowShadowSize;
            _renameBoxWindow.Top = Item.MainWindow.Top + _titleBarSize;
            _renameBoxWindow.Width = _renameBoxWindow.MaxWidth;
            _renameBoxWindow.Height = _renameBoxWindow.MaxHeight;

            this.IsVisible = true;
            Item.MainWindow.LocationChanged += MainWindow_LocationChanged;
            Item.MainWindow.SizeChanged += MainWindow_SizeChanged;
        }

        public void HideWindow()
        {
            if (_renameBoxWindow != null)
            {
                _renameBoxWindow.Hide();
                _renameBoxWindow.Close();
                _renameBoxWindow = null;
                Item.MainWindow.LocationChanged -= MainWindow_LocationChanged;
                Item.MainWindow.SizeChanged -= MainWindow_SizeChanged;
                this.IsVisible = false;
            }
        }

        public void SwitchMode(bool? toEnable = null)
        {
            if (toEnable == null) toEnable = !this.IsVisible;
            if (toEnable == true)
            {
                ShowWindow();
            }
            else
            {
                HideWindow();
            }
        }

        public void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            if (_renameBoxWindow != null && _renameBoxWindow.IsVisible)
            {
                _renameBoxWindow.Left = Item.MainWindow.Left + _windowShadowSize;
                _renameBoxWindow.Top = Item.MainWindow.Top + _titleBarSize;
            }
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double contentWidth = e.NewSize.Width - (_windowShadowSize * 2);
            double contentHeight = e.NewSize.Height - _titleBarSize - _windowShadowSize;

            _renameBoxWindow.Width = contentWidth <= _renameBoxWindow.MaxWidth ?
                contentWidth : _renameBoxWindow.MaxWidth;
            _renameBoxWindow.Height = contentHeight <= _renameBoxWindow.MaxHeight ?
                contentHeight : _renameBoxWindow.MaxHeight;
        }
    }
}

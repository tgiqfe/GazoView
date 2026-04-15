using GazoView.Lib.Panel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace GazoView.Lib
{
    public class RenameBox
    {
        private RenameBoxWindow _renameBoxWindow;
        private double _windowShadowSize = -1;
        private double _titleBarSize = -1;

        public bool IsVisibleRenameBox { get; set; }

        public void ShowWindow(double windowShadowSize, double titleBarSize)
        {
            _windowShadowSize = windowShadowSize;
            _titleBarSize = titleBarSize;

            _renameBoxWindow ??= new RenameBoxWindow();
            _renameBoxWindow.Owner = Item.MainWindow;
            _renameBoxWindow.Show();
            _renameBoxWindow.Left = Item.MainWindow.Left + _windowShadowSize;
            _renameBoxWindow.Top = Item.MainWindow.Top + _titleBarSize;

            this.IsVisibleRenameBox = true;
            Item.MainWindow.LocationChanged += MainWindow_LocationChanged;
        }

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

            this.IsVisibleRenameBox = true;
            Item.MainWindow.LocationChanged += MainWindow_LocationChanged;
        }


        public void HideWindow()
        {
            if (_renameBoxWindow != null)
            {
                _renameBoxWindow.Hide();
                _renameBoxWindow.Close();
                _renameBoxWindow = null;
                Item.MainWindow.LocationChanged -= MainWindow_LocationChanged;
                this.IsVisibleRenameBox = false;
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
    }
}

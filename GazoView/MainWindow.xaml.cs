using GazoView.Lib.Functions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GazoView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = Item.BindingParam;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
                case Key.Left:
                case Key.BrowserBack:
                    ChangeImage(-1);
                    break;
                case Key.Right:
                case Key.BrowserForward:
                    ChangeImage(1);
                    break;
                case Key.R:
                    ZoomImage();
                    break;
            }
        }

        /// <summary>
        /// Wheel event
        ///     ctrl + wheel -> zoom in/out.
        ///     wheel -> next/prev image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (SpecialKeyStatus.IsCtrPressed())
            {
                ZoomImage(e);
            }
            else
            {
                ChangeImage(e.Delta > 0 ? -1 : 1);
            }
        }


        private void ChangeImage(int direction)
        {
            Item.BindingParam.Images.Index += direction;
        }


        private void ZoomImage(MouseWheelEventArgs e = null)
        {
            if (e == null)
            {
                Item.ScaleRate.Reset();
            }
            else
            {
                int direction = e.Delta > 0 ? 1 : -1;
                Item.ScaleRate.Index += direction;
            }

            var scale = Item.ScaleRate.Scale;
            if (scale == 1)
            {
                MainImage.SetBinding(Image.WidthProperty, new Binding("ActualWidth") { Source = ScrollViewer });
                MainImage.SetBinding(Image.HeightProperty, new Binding("ActualHeight") { Source = ScrollViewer });
            }
            else
            {
                BindingOperations.ClearBinding(MainImage, Image.WidthProperty);
                BindingOperations.ClearBinding(MainImage, Image.HeightProperty);
                var newWidth = this.ActualWidth * scale;
                var newHeight = (this.ActualHeight - SystemParameters.WindowCaptionHeight) * scale;
                if (scale > 1)
                {
                    //  Before scaling, get pointer location.
                    Point mousePoint = e.GetPosition(ScrollViewer);
                    double viewX = ScrollViewer.HorizontalOffset;
                    double viewY = ScrollViewer.VerticalOffset;

                    MainImage.Width = newWidth;
                    MainImage.Height = newHeight;

                    //  Set the new size of the ScrollViewer. and move mouse pointer.
                    var relateScale = scale / Item.ScaleRate.PreviewScale;
                    ScrollViewer.ScrollToHorizontalOffset((mousePoint.X + viewX) * relateScale - mousePoint.X);
                    ScrollViewer.ScrollToVerticalOffset((mousePoint.Y + viewY) * relateScale - mousePoint.Y);
                }
                else if (scale < 1)
                {
                    MainImage.Width = newWidth;
                    MainImage.Height = newHeight;
                }
            }
        }
    }
}
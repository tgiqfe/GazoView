using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GazoView.Lib.Functions
{
    internal class ImageFunction
    {
        public static void ChangeImage(int direction)
        {
            Item.BindingParam.Images.Index += direction;
        }

        public static void ZoomImage(MainWindow mainWindow, Image mainImage, AdvancedScrollViewer scrollViewer, MouseWheelEventArgs e = null)
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
                mainImage.SetBinding(Image.WidthProperty, new Binding("ActualWidth") { Source = scrollViewer });
                mainImage.SetBinding(Image.HeightProperty, new Binding("ActualHeight") { Source = scrollViewer });
            }
            else
            {
                BindingOperations.ClearBinding(mainImage, Image.WidthProperty);
                BindingOperations.ClearBinding(mainImage, Image.HeightProperty);
                var newWidth = mainWindow.ActualWidth * scale;
                var newHeight = (mainWindow.ActualHeight - SystemParameters.WindowCaptionHeight) * scale;
                if (scale > 1)
                {
                    //  Before scaling, get pointer location.
                    Point mousePoint = e.GetPosition(scrollViewer);
                    double viewX = scrollViewer.HorizontalOffset;
                    double viewY = scrollViewer.VerticalOffset;

                    mainImage.Width = newWidth;
                    mainImage.Height = newHeight;

                    //  Set the new size of the ScrollViewer. and move mouse pointer.
                    var relateScale = scale / Item.ScaleRate.PreviewScale;
                    scrollViewer.ScrollToHorizontalOffset((mousePoint.X + viewX) * relateScale - mousePoint.X);
                    scrollViewer.ScrollToVerticalOffset((mousePoint.Y + viewY) * relateScale - mousePoint.Y);
                }
                else if (scale < 1)
                {
                    mainImage.Width = newWidth;
                    mainImage.Height = newHeight;
                }
            }
        }
    }
}

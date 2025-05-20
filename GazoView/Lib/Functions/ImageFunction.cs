using GazoView.Lib.Conf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GazoView.Lib.Functions
{
    internal class ImageFunction
    {
        public static void ChangeImage(int direction)
        {
            Item.BindingParam.Images.Index += direction;
        }

        public static void ZoomImage(MainWindow mainWindow, System.Windows.Controls.Image mainImage, AdvancedScrollViewer scrollViewer, MouseWheelEventArgs e = null)
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
                mainImage.SetBinding(System.Windows.Controls.Image.WidthProperty, new System.Windows.Data.Binding("ActualWidth") { Source = scrollViewer });
                mainImage.SetBinding(System.Windows.Controls.Image.HeightProperty, new System.Windows.Data.Binding("ActualHeight") { Source = scrollViewer });
            }
            else
            {
                BindingOperations.ClearBinding(mainImage, System.Windows.Controls.Image.WidthProperty);
                BindingOperations.ClearBinding(mainImage, System.Windows.Controls.Image.HeightProperty);
                var newWidth = mainWindow.ActualWidth * scale;
                var newHeight = (mainWindow.ActualHeight - SystemParameters.WindowCaptionHeight) * scale;
                if (scale > 1)
                {
                    //  Before scaling, get pointer location.
                    System.Windows.Point mousePoint = e.GetPosition(scrollViewer);
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

        public static void ImageFlip(Images images, bool isHorizontal)
        {
            var transform = isHorizontal ?
                new ScaleTransform(-1, 1, 0, 0) :
                new ScaleTransform(1, -1, 0, 0);
            var tb = images.Current.Source is TransformedBitmap ?
                new TransformedBitmap(images.Current.Source as TransformedBitmap, transform) :
                new TransformedBitmap(images.Current.Source as BitmapImage, transform);
            images.TempChangeImage(tb);
            tb.Freeze();
        }

        public static void ImageRotate(Images images)
        {
            var transform = new RotateTransform(90);
            var tb = images.Current.Source is TransformedBitmap ?
                new TransformedBitmap(images.Current.Source as TransformedBitmap, transform) :
                new TransformedBitmap(images.Current.Source as BitmapImage, transform);
            images.TempChangeImage(tb);
            tb.Freeze();
        }
    }
}

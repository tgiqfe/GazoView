using GazoView.Lib.Conf;
using System.IO;
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

            var scale = Item.MainBase.MainImage.ActualWidth / Item.BindingParam.Images.Current.Source.Width;
            /*
            Item.BindingParam.Trimming.Scale =
                Item.MainBase.MainImage.ActualWidth / Item.BindingParam.Images.Current.Source.Width;
            */
            Item.BindingParam.Trimming.Scale = scale;

            SwitchNearestNeighbor(scale >= 3);
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
                Item.BindingParam.State.ScalingMode = false;
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

        public static void ImageFlip(Images images, bool isHorizontal)
        {
            var transform = isHorizontal ?
                new ScaleTransform(-1, 1, 0, 0) :
                new ScaleTransform(1, -1, 0, 0);
            var tb = new TransformedBitmap(images.Current.Source as BitmapSource, transform);
            images.TempChangeImage(tb);
            tb.Freeze();
        }

        public static void ImageRotate(Images images)
        {
            var transform = new RotateTransform(90);
            var tb = new TransformedBitmap(images.Current.Source as BitmapSource, transform);
            images.TempChangeImage(tb);
            tb.Freeze();
        }

        


        public static void SwitchNearestNeighbor(bool? toEnable = null)
        {
            var isEnable = RenderOptions.GetBitmapScalingMode(Item.MainBase.MainImage) == BitmapScalingMode.NearestNeighbor;
            toEnable ??= !isEnable;

            RenderOptions.SetBitmapScalingMode(Item.MainBase.MainImage,
                toEnable.Value ?
                    BitmapScalingMode.NearestNeighbor :
                    BitmapScalingMode.HighQuality);
        }

        public static void SwitchTrimmingMode(bool? toenable = null)
        {
            if (Item.BindingParam.Trimming.Top < 0)
            {
                Item.BindingParam.Trimming.Top = 100;
            }
            if (Item.BindingParam.Trimming.Bottom < 0)
            {
                Item.BindingParam.Trimming.Bottom = 300;
            }
            if (Item.BindingParam.Trimming.Left < 0)
            {
                Item.BindingParam.Trimming.Left = 100;
            }
            if (Item.BindingParam.Trimming.Right < 0)
            {
                Item.BindingParam.Trimming.Right = 300;
            }

            Item.BindingParam.State.TrimmingMode =
                toenable ?? !Item.BindingParam.State.TrimmingMode;
        }

        public static void StartTrimming()
        {
            if (Item.BindingParam.State.TrimmingMode)
            {
                string output = FilePaths.Deduplicate(Item.BindingParam.Images.Current.FilePath);

                var ret = MessageBox.Show($"Trim.\r\n[ {output} ]",
                    Item.ProcessName,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information,
                    MessageBoxResult.Yes);
                if (ret != MessageBoxResult.Yes) return;

                var (left, top, width, height) = (
                    Item.BindingParam.Trimming.Left,
                    Item.BindingParam.Trimming.Top,
                    Item.BindingParam.Trimming.Right - Item.BindingParam.Trimming.Left,
                    Item.BindingParam.Trimming.Bottom - Item.BindingParam.Trimming.Top);
                
                if(Item.BindingParam.Images.Current.Source is BitmapSource imgSrc)
                {
                    var bitmap = new CroppedBitmap(imgSrc, new Int32Rect(left, top, width, height));
                    using(var fs = new FileStream(output, FileMode.Create, FileAccess.Write))
                    {
                        BitmapEncoder encoder = Item.BindingParam.Images.Current.FileExtension.ToLower() switch
                        {
                            ".jpg" or ".jpeg" => new JpegBitmapEncoder(),
                            ".png" => new PngBitmapEncoder(),
                            ".gif" => new GifBitmapEncoder(),
                            ".tif" or ".tiff" => new TiffBitmapEncoder(),
                            ".bmp" => new BmpBitmapEncoder(),
                            _ => null,
                        };
                        encoder.Frames.Add(BitmapFrame.Create(bitmap));
                        encoder.Save(fs);
                    }
                    bitmap.Freeze();
                }
            }
        }
    }
}

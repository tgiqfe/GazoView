using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GazoView.Lib.ImageInfo
{
    internal class ImageItem_Bitmap : ImageItem
    {
        const double DPI_96 = 96;

        public ImageItem_Bitmap(string path) : base(path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.CreateOptions = BitmapCreateOptions.None;
                bitmap.StreamSource = fs;
                bitmap.EndInit();
                bitmap.Freeze();

                this.Width = bitmap.PixelWidth;
                this.Height = bitmap.PixelHeight;
                this.DpiX = bitmap.DpiX;
                this.DpiY = bitmap.DpiY;

                if (DpiX == DPI_96 && DpiY == DPI_96)
                {
                    this.Source = bitmap;
                }
                else
                {
                    //  DPIが96x96以外の場合は、96x96に変換する
                    int stride = Width * (bitmap.Format.BitsPerPixel / 8);
                    byte[] pixels = new byte[Height * stride];
                    bitmap.CopyPixels(pixels, stride, 0);
                    this.Source = BitmapSource.Create(Width, Height, DPI_96, DPI_96, PixelFormats.Pbgra32, null, pixels, stride);
                }
            }
        }
    }
}

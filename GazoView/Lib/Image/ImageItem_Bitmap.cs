using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GazoView.Lib.Image
{
    internal class ImageItem_Bitmap : ImageItem
    {
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

                this.Source = bitmap;
                this.Width = bitmap.PixelWidth;
                this.Height = bitmap.PixelHeight;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GazoView.Lib.Config
{
    internal class BitmapImageItem : IImageItem
    {
        public string FileName { get; private set; }

        public string FilePath { get; private set; }

        public string FileExtension { get; private set; }

        public ImageSource Source { get; private set; }

        public double Width { get; private set; }

        public double Height { get; private set; }

        public BitmapImageItem(string path)
        {
            this.FileName = Path.GetFileName(path);
            this.FilePath = path;
            this.FileExtension = Path.GetExtension(path);
            (this.Source, this.Width, this.Height) = GetImageSource(path);
        }

        private (BitmapImage, double, double) GetImageSource(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.CreateOptions = BitmapCreateOptions.None;
                bmp.StreamSource = fs;
                bmp.EndInit();
                bmp.Freeze();

                return (bmp, bmp.PixelWidth, bmp.PixelHeight);
            }
        }
    }
}

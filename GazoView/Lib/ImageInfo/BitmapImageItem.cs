using System.IO;
using System.Windows.Media.Imaging;

namespace GazoView.Lib.ImageInfo
{
    internal class BitmapImageItem : BaseImageItem
    {
        public BitmapImageItem(string path) : base(path)
        {
            (this.Source, this.Width, this.Height) = GetImageSource(path);
        }

        /// <summary>
        /// イメージ情報(ソース、幅、高さ)を取得する
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private (BitmapImage, double, double) GetImageSource(string path)
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
                return (bitmap, bitmap.PixelWidth, bitmap.PixelHeight);
            }
        }
    }
}

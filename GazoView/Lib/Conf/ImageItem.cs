using System.Security.Cryptography;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace GazoView.Lib.Conf
{
    internal class ImageItem
    {
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        public string FileExtension { get; private set; }
        public string Parent { get; private set; }
        public string Size { get; private set; }
        public string LastWriteTime { get; private set; }
        public string Hash { get; private set; }

        public ImageSource Source { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public double DpiX { get; private set; }
        public double DpiY { get; private set; }

        public ImageItem(string path)
        {
            FilePath = path;
            FileName = Path.GetFileName(path);
            FileExtension = Path.GetExtension(path);
            Parent = Path.GetDirectoryName(path);
            Size = new FileInfo(path).Length switch
            {
                long s when s < 1024 =>
                    $"{s} Byte",
                long s when s < 1024 * 1024 =>
                    $"{Math.Round(s / 1024D, 2, MidpointRounding.AwayFromZero)} KB",
                long s when s < 1024 * 1024 * 1024 =>
                    $"{Math.Round(s / 1024D / 1024D, 2, MidpointRounding.AwayFromZero)} MB",
                long s when s < 1024L * 1024L * 1024L * 1024L =>
                    $"{Math.Round(s / 1024D / 1024D / 1024D, 2, MidpointRounding.AwayFromZero)} GB",
                long s when s < 1024L * 1024L * 1024L * 1024L * 1024L =>
                    $"{Math.Round(s / 1024D / 1024D / 1024D / 1024D, 2, MidpointRounding.AwayFromZero)} TB",
                _ => "",
            };
            LastWriteTime = File.GetLastWriteTime(path).ToString("yyyy/MM/dd HH:mm:ss");
            Hash = new Func<string, string>(_path =>
            {
                using (var fs = new FileStream(_path, FileMode.Open, FileAccess.Read))
                {
                    var md5 = MD5.Create();
                    byte[] bytes = md5.ComputeHash(fs);
                    md5.Clear();
                    return BitConverter.ToString(bytes).Replace("-", "");
                }
            })(path);

            switch (FileExtension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".tif":
                case ".tiff":
                case ".bmp":
                    SetBitmapSource();
                    break;
            }
        }

        private void SetBitmapSource()
        {
            double DPI_96 = 96.0;
            using (var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.CreateOptions = BitmapCreateOptions.None;
                bitmap.StreamSource = fs;
                bitmap.EndInit();
                bitmap.Freeze();

                Width = bitmap.PixelWidth;
                Height = bitmap.PixelHeight;
                DpiX = bitmap.DpiX;
                DpiY = bitmap.DpiY;
                Source = DpiX == DPI_96 && DpiY == DPI_96 ?
                    bitmap :
                    new Func<BitmapSource>(() =>
                    {
                        int stride = Width * (bitmap.Format.BitsPerPixel / 8);
                        byte[] pixels = new byte[Height * stride];
                        bitmap.CopyPixels(pixels, stride, 0);
                        return BitmapSource.Create(Width, Height, DPI_96, DPI_96, PixelFormats.Pbgra32, null, pixels, stride);
                    })();
            }
        }
    }
}

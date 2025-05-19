using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

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

        private static Regex pattern_starFile = new Regex(@"★\.[^\.]+$");
        public bool IsStar { get; private set; }

        public ImageSource Source { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public double DpiX { get; private set; }
        public double DpiY { get; private set; }

        public ImageItem(string path)
        {
            this.FilePath = path;
            this.FileName = Path.GetFileName(path);
            this.FileExtension = Path.GetExtension(path);
            this.Parent = Path.GetDirectoryName(path);
            this.Size = new FileInfo(path).Length switch
            {
                long size when size < 1024 =>
                    $"{size} Byte",
                long size when size < 1024 * 1024 =>
                    $"{Math.Round(size / 1024D, 2, MidpointRounding.AwayFromZero)} KB",
                long size when size < 1024 * 1024 * 1024 =>
                    $"{Math.Round(size / 1024D / 1024D, 2, MidpointRounding.AwayFromZero)} MB",
                long size when size < 1024L * 1024L * 1024L * 1024L =>
                    $"{Math.Round(size / 1024D / 1024D / 1024D, 2, MidpointRounding.AwayFromZero)} GB",
                long size when size < 1024L * 1024L * 1024L * 1024L * 1024L =>
                    $"{Math.Round(size / 1024D / 1024D / 1024D / 1024D, 2, MidpointRounding.AwayFromZero)} TB",
                _ => ""
            };
            this.LastWriteTime = File.GetLastWriteTime(path).ToString("yyyy/MM/dd HH:mm:ss");
            this.Hash = new Func<string, string>(_path =>
            {
                using (var fs = new FileStream(_path, FileMode.Open, FileAccess.Read))
                {
                    var md5 = MD5.Create();
                    byte[] bytes = md5.ComputeHash(fs);
                    md5.Clear();
                    return BitConverter.ToString(bytes).Replace("-", "");
                }
            })(path);
            this.IsStar = pattern_starFile.IsMatch(FileName);

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

                this.Width = bitmap.PixelWidth;
                this.Height = bitmap.PixelHeight;
                this.DpiX = bitmap.DpiX;
                this.DpiY = bitmap.DpiY;
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

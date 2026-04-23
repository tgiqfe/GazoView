using GazoView.Lib.Functions;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GazoView.Lib
{
    public class ImageItem
    {
        private static readonly string[] _bitmapExtensions = new string[]
        {
            ".jpg", ".jpeg", ".png", ".bmp", ".tiff", ".tif", ".webp",
        };
        private static readonly string[] _vectorExtensions = new string[]
        {
            ".svg",
        };

        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        public string FileExtension { get; private set; }
        public string Parent { get; private set; }
        public string Size { get => FileFunction.GetFileSize(new FileInfo(this.FilePath).Length); }
        public DateTime LastWriteTimeRaw { get; private set; }
        public string LastWriteTime { get => this.LastWriteTimeRaw.ToString("yyyy/MM/dd HH:mm:ss"); }
        public string Hash { get => FileFunction.GetHash(this.FilePath); }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Resolution { get => $"{Width} x {Height}"; }
        public double DpiX { get; private set; }
        public double DpiY { get; private set; }

        public ImageSource Source { get; private set; }

        public ImageItem(string path)
        {
            this.FilePath = path;
            this.FileName = Path.GetFileName(path);
            this.FileExtension = Path.GetExtension(path);
            this.Parent = Path.GetDirectoryName(path);
            //this.Size = FileFunction.GetFileSize(new FileInfo(path).Length);
            this.LastWriteTimeRaw = File.GetLastWriteTime(path);
            //this.LastWriteTime = this.LastWriteTimeRaw.ToString("yyyy/MM/dd HH:mm:ss");
            //this.Hash = FileFunction.GetHash(path);

            if (_bitmapExtensions.Any(x => string.Equals(x, this.FileExtension, StringComparison.OrdinalIgnoreCase)))
            {
                double DPI_96 = 96.0;
                using (var fs = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read))
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
                    this.Source = DpiX == DPI_96 && DpiY == DPI_96 ?
                        bitmap :
                        new Func<BitmapSource>(() =>
                        {
                            int stride = Width * (bitmap.Format.BitsPerPixel / 8);
                            byte[] pixels = new byte[Height * stride];
                            bitmap.CopyPixels(pixels, stride, 0);
                            bitmap.Freeze();    //  Freeze the original bitmap to release the file lock.    
                            return BitmapSource.Create(Width, Height, DPI_96, DPI_96, PixelFormats.Pbgra32, null, pixels, stride);
                        })();
                }
            }
            else if (_vectorExtensions.Any(x => string.Equals(x, this.FileExtension, StringComparison.OrdinalIgnoreCase)))
            {
                //  SVG rendering is not implemented yet.
            }
        }
    }
}
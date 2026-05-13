using GazoView.Lib.Functions;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GazoView.Lib
{
    public class ImageItem
    {
        #region valid image extensions

        private static readonly string[] _bitmapExtensions = new string[]
        {
            ".jpg", ".jpeg", ".png", ".bmp", ".tiff", ".tif", ".webp",
        };
        private static readonly string[] _vectorExtensions = new string[]
        {
            ".svg",
        };

        #endregion

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

        /// <summary>
        /// Create empty image.
        /// </summary>
        public ImageItem()
        {
            this.FilePath = "-";
            this.FileName = "-";
            this.FileExtension = string.Empty;
            this.Parent = string.Empty;
            this.LastWriteTimeRaw = DateTime.MinValue;
            this.Source = null;
        }

        /// <summary>
        /// Read image file and create image item.
        /// </summary>
        /// <param name="path">The path of the</param>
        public ImageItem(string path)
        {
            this.FilePath = path;
            this.FileName = Path.GetFileName(path);
            this.FileExtension = Path.GetExtension(path);
            this.Parent = Path.GetDirectoryName(path);
            this.LastWriteTimeRaw = File.GetLastWriteTime(path);

            if (_bitmapExtensions.Any(x => string.Equals(x, this.FileExtension, StringComparison.OrdinalIgnoreCase)))
            {
                double DPI_96 = 96.0;
                BitmapImage bitmap = null;
                int retryCount = 3;
                int retryDelay = 100;

                for (int i = 0; i < retryCount; i++)
                {
                    try
                    {
                        using (var fs = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.CreateOptions = BitmapCreateOptions.None;
                            bitmap.StreamSource = fs;
                            bitmap.EndInit();
                            bitmap.Freeze();
                        }
                        break;
                    }
                    catch (IOException) when (i < retryCount - 1)
                    {
                        System.Threading.Thread.Sleep(retryDelay);
                    }
                    catch (IOException)
                    {
                        this.Width = 0;
                        this.Height = 0;
                        this.DpiX = 0;
                        this.DpiY = 0;
                        this.Source = null;
                        return;
                    }
                }

                if (bitmap != null)
                {
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
                            bitmap.Freeze();
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
using SharpVectors.Converters;
using SharpVectors.Renderers;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Xml;
using System.Xml.Linq;
using XamlAnimatedGif;

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

        public ImageItem(string path, ImageSource source = null)
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

            if (source == null)
            {
                switch (FileExtension.ToLower())
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".tif":
                    case ".tiff":
                    case ".bmp":
                    case ".webp":
                        SetBitmapSource();
                        break;
                    case ".svg":
                        SetVectorSource();
                        break;
                        /*
                    case ".gif":
                        SetAnimationSource();
                        break;
                        */
                }
            }
            else
            {
                this.Source = source;
            }
        }

        private void SetBitmapSource()
        {
            /*
            if (Item.BindingParam != null)
            {
                if (Item.BindingParam.State.IsGifFile)
                {
                    AnimationBehavior.SetSourceUri(Item.MainBase.MainImage, null);
                    
                }
            }
            */

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
                this.Source = DpiX == DPI_96 && DpiY == DPI_96 ?
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

        private void SetVectorSource()
        {
            var xml = XElement.Load(this.FilePath);
            var xmLWidth = xml.Attributes().FirstOrDefault(x =>
                x.Name.ToString().Equals("Width", StringComparison.OrdinalIgnoreCase));
            this.Width = int.TryParse(xmLWidth?.Value, out int width) ? width : -1;
            var xmLHeight = xml.Attributes().FirstOrDefault(x =>
                x.Name.ToString().Equals("Height", StringComparison.OrdinalIgnoreCase));
            this.Height = int.TryParse(xmLHeight?.Value, out int height) ? height : -1;
            this.DpiX = -1;
            this.DpiY = -1;

            WpfDrawingSettings settings = new();
            settings.IncludeRuntime = true;
            settings.TextAsGeometry = false;
            using (var converter = new StreamSvgConverter(settings))
            using (var ms = new MemoryStream())
            {
                if (converter.Convert(this.FilePath, ms))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    this.Source = bitmap;
                }
            }
        }

        private void SetAnimationSource()
        {
            //this.Source = null;
            Item.BindingParam.State.IsGifFile = true;

            using (var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                /*
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.CreateOptions = BitmapCreateOptions.None;
                bitmap.StreamSource = fs;
                bitmap.EndInit();
                bitmap.Freeze();

                this.Width = bitmap.PixelWidth;
                this.Height = bitmap.PixelHeight;
                this.DpiX = -1;
                this.DpiY = -1;
                */

                //AnimationBehavior.SetSourceStream(Item.MainBase.MainImage, fs);
            }
            AnimationBehavior.SetSourceUri(Item.MainBase.MainImage, new Uri(FilePath));
            
        }

        /*
        private void SetAnimationSource()
        {
            this.Source = null;
            Item.BindingParam.State.IsGifFile = true;

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
                this.DpiX = -1;
                this.DpiY = -1;

                ImageBehavior.SetAnimatedSource(Item.MainBase.MainImage, bitmap);
            }
        }
        */
    }
}

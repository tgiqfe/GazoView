using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GazoView.Lib.Config
{
    internal class BitmapImageItem : IImageItem
    {
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileExtension { get; set; }

        public string LabelFileName { get; private set; }

        public string LabelFilePath { get; private set; }

        public string LabelFileExtension { get; private set; }

        public ImageSource Source { get; private set; }

        public double Width { get; private set; }

        public double Height { get; private set; }

        public string Size { get; private set; }

        public string LastWriteTime { get; private set; }

        public BitmapImageItem(string path)
        {
            this.FileName = Path.GetFileName(path);
            this.FilePath = path;
            this.FileExtension = Path.GetExtension(path);
            this.LabelFileName = FileName.Contains("_") ? FileName.Replace("_", "__") : FileName;
            this.LabelFilePath = FilePath.Contains("_") ? FilePath.Replace("_", "__") : FilePath;
            this.LabelFileExtension = FileExtension.Contains("_") ? FileExtension.Replace("_", "__") : FileExtension;

            (this.Source, this.Width, this.Height) = GetImageSource(path);
            this.Size = GetFileSize(path);
            this.LastWriteTime = File.GetLastWriteTime(path).ToString("yyyy/MM/dd HH:mm:ss");
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

        private string GetFileSize(string path)
        {
            long size = new FileInfo(path).Length;
            if (size < 1024)
            {
                return $"{size} Byte";
            }
            else if (size < (1024 * 1024))
            {
                var res = Math.Round(size / 1024D, 2, MidpointRounding.AwayFromZero);
                return $"{res} KB";
            }
            else if (size < (1024 * 1024 * 1024))
            {
                var res = Math.Round(size / 1024D / 1024D, 2, MidpointRounding.AwayFromZero);
                return $"{res} MB";
            }
            else if (size < (1024L * 1024 * 1024 * 1024))
            {
                var res = Math.Round(size / 1024D / 1024D / 1024D, 2, MidpointRounding.AwayFromZero);
                return $"{res} GB";
            }
            else if (size < (1024L * 1024 * 1024 * 1024 * 1024))
            {
                var res = Math.Round(size / 1024D / 1024D / 1024D / 1024D, 2, MidpointRounding.AwayFromZero);
                return $"{res} TB";
            }

            return "";
        }
    }
}

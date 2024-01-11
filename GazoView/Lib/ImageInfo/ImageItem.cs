using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GazoView.Lib.ImageInfo
{
    internal class ImageItem
    {
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        public string FileExtension { get; private set; }
        
        public string Size { get; private set; }
        public string LastWriteTime { get; private set; }
        public string Hash { get; private set; }

        public ImageSource Source { get; protected set; }
        public double Width { get; protected set; }
        public double Height { get; protected set; }
        public double ViewWidth { get; protected set; }
        public double ViewHeight { get; protected set; }

        public ImageItem(string path)
        {
            this.FilePath = path;
            this.FileName = Path.GetFileName(path);
            this.FileExtension = Path.GetExtension(path);
            this.Size = GetFileSize(path);
            this.Hash = GetHash(path);
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

        private string GetHash(string path)
        {
            string ret = null;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var md5 = MD5.Create();
                byte[] bytes = md5.ComputeHash(fs);
                ret = BitConverter.ToString(bytes);
                md5.Clear();
            }
            return ret;
        }
    }
}

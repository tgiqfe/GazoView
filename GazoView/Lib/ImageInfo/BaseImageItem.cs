using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.IO;
using System.Security.Cryptography;
using System.Security.Permissions;

namespace GazoView.Lib.ImageInfo
{
    internal class BaseImageItem : IImageItem
    {
        public string FilePath { get; private set; }

        public string FileName { get; private set; }

        public string FileExtension { get; private set; }

        public string LabelFileName { get; private set; }

        public string LabelFilePath { get; private set; }

        public string LabelFileExtension { get; private set; }

        public string Size { get; private set; }

        public string LastWriteTime { get; private set; }

        public string Hash { get; private set; }

        public ImageSource Source { get; protected set; }

        public double Width { get; protected set; }

        public double Height { get; protected set; }

        public double Scale { get { return _ticks[TickIndex]; } }



        protected static readonly double[] _ticks = new double[]
        {
            0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.8, 2, 2.4, 2.8, 3.2, 3.6, 4, 4.8, 5.6, 6.4, 7.2, 8
        };

        private int _tickindex = 8;

        public int TickIndex
        {
            get { return _tickindex; }
            set
            {
                _tickindex = value;
                if (_tickindex < 0) _tickindex = 0;
                if (_tickindex >= _ticks.Length) _tickindex = _ticks.Length - 1;
            }
        }





        public BaseImageItem(string path)
        {
            this.FilePath = path;
            this.FileName = Path.GetFileName(path);
            this.FileExtension = Path.GetExtension(path).ToLower();
            this.LabelFileName = FileName.Contains("_") ? FileName.Replace("_", "__") : FileName;
            this.LabelFilePath = FilePath.Contains("_") ? FilePath.Replace("_", "__") : FilePath;
            this.LabelFileExtension = FileExtension.Contains("_") ? FileExtension.Replace("_", "__") : FileExtension;
            this.Size = GetFileSize(path);
            this.LastWriteTime = File.GetLastWriteTime(path).ToString("yyyy/MM/dd HH:mm:ss");
            this.Hash = GetHash(path);
        }

        /// <summary>
        /// ファイルサイズを取得する
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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

        /// <summary>
        /// ファイルのハッシュ値を取得する
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace GazoView.Lib.Functions
{
    internal class ImageTrimming
    {
        public BitmapSource ImageSource { get; set; }

        public string OutputPath { get; set; }

        public string Extension { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool Enabled { get; set; }
        public bool Success { get; set; }

        public ImageTrimming(ImageSource imageSource, string path, int x, int y, int width, int height)
        {
            this.OutputPath = FileAction.CreateSafePath(path);
            this.Extension = Path.GetExtension(path).ToLower();
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;

            if (imageSource is BitmapSource)
            {
                this.ImageSource = imageSource as BitmapSource;
                this.Enabled = true;
            }
        }

        public void Cut()
        {
            var bitmap = new CroppedBitmap(ImageSource, new Int32Rect(X, Y, Width, Height));

            using (var fs = new FileStream(OutputPath, FileMode.Create, FileAccess.Write))
            {
                BitmapEncoder imageEnc = Extension switch
                {
                    ".jpeg" => new JpegBitmapEncoder(),
                    ".jpg" => new JpegBitmapEncoder(),
                    ".png" => new PngBitmapEncoder(),
                    ".bmp" => new BmpBitmapEncoder(),
                    _ => null,
                };
                imageEnc.Frames.Add(BitmapFrame.Create(bitmap));
                imageEnc.Save(fs);
            }
        }
    }
}

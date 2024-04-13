using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GazoView.Lib.Function
{
    internal class ImageTrimming
    {
        /// <summary>
        /// 画像をトリミングして結果を保存
        /// </summary>
        /// <param name="imageSource"></param>
        /// <param name="path"></param>
        /// <param name="extension"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void Cut(
            ImageSource imageSource,
            string path,
            string outputPath,
            string extension,
            int left, int top, int width, int height)
        {
            if (imageSource is BitmapSource imgSrc)
            {
                var bitmap = new CroppedBitmap(imgSrc, new System.Windows.Int32Rect(left, top, width, height));
                using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    BitmapEncoder encoder = extension.ToLower() switch
                    {
                        ".jpg" or ".jpeg" => new JpegBitmapEncoder(),
                        ".png" => new PngBitmapEncoder(),
                        ".tif" or ".tiff" => new TiffBitmapEncoder(),
                        ".bmp" => new BmpBitmapEncoder(),
                        _ => null,
                    };
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    encoder.Save(fs);
                }
            }
        }
    }
}

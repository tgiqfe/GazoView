using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;

namespace GazoView.Functions
{
    /// <summary>
    /// トリミング実行用
    /// </summary>
    class ImageTrimming
    {
        /// <summary>
        /// トリミング開始
        /// </summary>
        /// <param name="imageSource"></param>
        /// <param name="imagePath"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string Cut(BitmapSource imageSource, string imagePath, double x, double y, double width, double height)
        {
            var bitmap = new CroppedBitmap(imageSource, new Int32Rect((int)x, (int)y, (int)width, (int)height));

            string extension = Path.GetExtension(imagePath);
            string outputPath = SearchUnusedPath(
                Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath)),
                1,
                extension);
            using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                switch (extension.ToLower())
                {
                    case ".jpg":
                    case ".jpeg":
                        BitmapEncoder jpgEnc = new JpegBitmapEncoder();
                        jpgEnc.Frames.Add(BitmapFrame.Create(bitmap));
                        jpgEnc.Save(fs);
                        break;
                    case ".png":
                        BitmapEncoder pngEnc = new PngBitmapEncoder();
                        pngEnc.Frames.Add(BitmapFrame.Create(bitmap));
                        pngEnc.Save(fs);
                        break;
                    case ".bmp":
                    default:
                        BitmapEncoder bmpEnc = new BmpBitmapEncoder();
                        bmpEnc.Frames.Add(BitmapFrame.Create(bitmap));
                        bmpEnc.Save(fs);
                        break;
                }
            }

            return outputPath;
        }

        /// <summary>
        /// 未使用の名前を探して返す
        /// </summary>
        /// <param name="nameWithoutExt"></param>
        /// <param name="number"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        private static string SearchUnusedPath(string nameWithoutExt, int number, string extension)
        {
            string tempName = string.Format("{0}_{1}{2}", nameWithoutExt, number, extension);
            if (File.Exists(tempName))
            {
                return SearchUnusedPath(nameWithoutExt, number + 1, extension);
            }
            return tempName;
        }
    }
}

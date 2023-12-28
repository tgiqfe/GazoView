using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.ImageInfo
{
    class ImageItemGenerator
    {
        public static IImageItem Create(string path)
        {
            var extension = System.IO.Path.GetExtension(path).ToLower();
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".tif":
                case ".tiff":
                case ".bmp":
                    return new BitmapImageItem(path);
            }

            //".jpg", ".jpeg", ".png", ".tif", ".tiff", ".svg", ".bmp", ".gif"
            return null;
        }
    }
}

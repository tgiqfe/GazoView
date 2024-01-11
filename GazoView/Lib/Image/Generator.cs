using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Image
{
    internal class Generator
    {
        public static ImageItem Create(string path)
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
                    return new ImageItem_Bitmap(path);
            }

            return null;
        }
    }
}

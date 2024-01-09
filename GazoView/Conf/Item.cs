using GazoView.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Conf
{
    class Item
    {
        public const string ProcessName = "GazoView";

        public static BindingParam BindingParam = null;

        public static MainWindow Mainbase = null;

        public static TrimmingLayer TrimLayer = null;

        /// <summary>
        /// 許可する拡張子
        /// </summary>
        public static string[] ValidExtensions = new string[]
        {
            ".jpg", ".jpeg", ".png", ".tif", ".tiff",".bmp"
        };

        /*
         * 将来追加予定
         * - .svg
         * - .gif
         * - .ico
         * - .webp
         */
    }
}

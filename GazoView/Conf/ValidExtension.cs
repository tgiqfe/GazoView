﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Conf
{
    class ValidExtension
    {
        /// <summary>
        /// 許可する拡張子
        /// </summary>
        public static string[] Names = new string[]
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

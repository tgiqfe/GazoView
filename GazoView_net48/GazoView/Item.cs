using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GazoView.Config;

namespace GazoView
{
    class Item
    {
        /// <summary>
        /// Binding用のパラメータを格納
        /// </summary>
        public static BindingData Data = null;

        /// <summary>
        /// イメージ管理用
        /// </summary>
        public static ImageStore ImageStore = null;
    }
}

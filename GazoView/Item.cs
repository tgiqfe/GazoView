using GazoView.Lib.Config;
using GazoView.Lib.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GazoView
{
    /// <summary>
    /// 静的パラメータを格納
    /// </summary>
    class Item
    {
        public const string ProcessName = "GazoView";

        public static BindingParam BindingParam = null;

        public static MainWindow MainBase = null;

        public static ProcessManager ProcessManager = null;

        public static FilePath FilePath = null;

        public static FileWatcher FileWatcher = null;

        /// <summary>
        /// 拡縮モードで右クリック移動する場合の開始位置
        /// </summary>
        public static Point StartPoint_RightButtonMove;

        /// <summary>
        /// 拡縮モードで右クリック移動する場合の開始位置
        /// </summary>
        public static Point StartPosition_RightButtonMove;

    }
}

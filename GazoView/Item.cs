using GazoView.Lib.Conf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GazoView
{
    internal class Item
    {
        public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static readonly string WorkDirectory = Path.Combine(
            Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));

        public static BindingParam BindingParam = null;

        public static MainWindow MainBase = null;

        public static ScaleRate ScaleRate = null;
    }
}

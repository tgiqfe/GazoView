using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Conf
{
    internal class Item
    {
        public static readonly string WorkDirectory = Path.Combine(
            Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));

        public static BindingParam BindingParam = null;

        public static ScaleRate ScaleRate = null;
    }
}

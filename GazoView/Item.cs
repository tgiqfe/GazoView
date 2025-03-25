using GazoView.Lib.Conf;
using System.Diagnostics;
using System.IO;
using System.Windows.Data;

namespace GazoView
{
    internal class Item
    {
        public const string ProcessName = "GazoView";

        public static readonly string WorkingDirectory = Path.Combine(
            Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));

        public static BindingParam BindingParam = null;

        public static MainWindow MainBase = null;
    }
}

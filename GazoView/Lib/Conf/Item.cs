using System.Diagnostics;
using System.IO;

namespace GazoView.Lib.Conf
{
    internal class Item
    {
        public const string ProcessName = "GazoView";

        public static readonly string WorkingDirectory = Path.Combine(
            Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));

        public static BindingParam BindingParam = null;

        public static MainWindow MainBase = null;

        public static InfoPanel InfoPanel = null;
    }
}

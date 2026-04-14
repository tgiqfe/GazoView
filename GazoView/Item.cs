using GazoView.Lib;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace GazoView
{
    class Item
    {
        /// <summary>
        /// Process name of GazoView. This is used for IPC and other purposes.
        /// </summary>
        public const string ProcessName = "GazoView";

        /// <summary>
        /// Application version.
        /// v0.6.*
        /// </summary>
        public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Working directory.
        /// Same location as the executable file of GazoView. 
        /// </summary>
        public static readonly string WorkDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        /// <summary>
        /// MainWindow instance.
        /// This is set when MainWindow is created and can be used for IPC and other purposes.
        /// </summary>
        internal static MainWindow MainWindow = null;


        public static BindingParam BindingParam = null;

        /// <summary>
        /// MainWindow loaded flag.
        /// </summary>
        internal static bool IsInitialized = false;
    }
}

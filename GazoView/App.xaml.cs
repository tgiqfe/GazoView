using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace GazoView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Application startup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool isDebug = false;
#if DEBUG
            isDebug = true;
#endif
            var imageFileTargets = e.Args;
            if (isDebug) imageFileTargets = new string[] { @"D:\Test\Images" };

            Item.BindingParam = new()
            {
                Setting = Setting.Load(),
                Images = new(imageFileTargets),
                Trimming = new()
            };
        }

        /// <summary>
        /// Application exit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Item.BindingParam.Images?.Dispose();
            Item.BindingParam.Setting?.Save();
        }
    }
}

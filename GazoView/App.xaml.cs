using System.Configuration;
using System.Data;
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
            if (isDebug)
            {
                Item.BindingParam = new()
                {
                    Setting = Setting.Load(),
                    Images = new(new string[] { @"D:\Test\Images" }),
                };
            }
            else
            {
                Item.BindingParam = new()
                {
                    Setting = Setting.Load(),
                    Images = new(e.Args),
                };
            }
        }

        /// <summary>
        /// Application exit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Item.BindingParam.Setting.Save();
        }
    }
}

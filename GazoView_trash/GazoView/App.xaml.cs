using GazoView.Conf;
using GazoView.Lib;
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
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Item.BindingParam = new BindingParam(e.Args);
            Item.BindingParam = new BindingParam(new string[] { @"D:\Test\Images" });
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Item.BindingParam.Setting.Save();
        }
    }

}

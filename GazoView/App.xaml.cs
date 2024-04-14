using GazoView.Lib.Conf;
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
            Item.BindingParam = new()
            {
                Setting = Setting.Load(),
                //Images = new(new string[] { @"D:\Test\Images\001.png" }),
                Images = new(e.Args),
                State = new(),
                Trimming = new(),
            };
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Item.BindingParam.Setting.Save();
        }
    }

}

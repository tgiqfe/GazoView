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
            var setting = Setting.Load();
            Item.BindingParam = new BindingParam()
            {
                Setting = setting,
                Images = new(e.Args),
                State = new(),
                Trimming = new()
            };
            Item.ScaleRate = new ScaleRate();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Item.BindingParam.Setting.Save();
            Item.DeletedStore?.Close();
        }
    }
}

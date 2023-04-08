using GazoView.Lib.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            Item.BindingParam = new BindingParam()
            {
                Images = new ImageStore(new string[] { @"D:\Test\sample" }),
                //Collection = new ImageCollection(e.Args),
            };
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Item.BindingParam.Close();
        }
    }
}

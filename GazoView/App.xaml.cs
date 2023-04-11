using GazoView.Lib.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GazoView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// アプリケーション起動時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //  設定ファイル等のパスの管理
            Item.FilePath = new();

            //  起動プロセスの管理
            bool isCtrlDown =
                (Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down;
            Item.ProcessManager = new(e.Args, isCtrlDown);
            if (!Item.ProcessManager.Enabled)
            {
                Application.Current.Shutdown();
            }

            //  ファイル増減監視
            Item.FileWatcher = new FileWatcher();

            //  画像をセット
            Item.BindingParam = new BindingParam();
            Item.BindingParam.Images = new ImageStore(e.Args);
        }

        /// <summary>
        /// アプリケーション終了時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Item.FileWatcher.StopFileListUpdate();
            Item.BindingParam.Close();
            Item.ProcessManager.Close();
        }
    }
}

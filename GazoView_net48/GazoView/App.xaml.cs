using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using GazoView.Config;
using System.Windows.Input;
using System.Threading;
using GazoView.Functions;

namespace GazoView
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private Mutex mutex = null;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //  設定ファイルの読み込み
            Item.Data = new BindingData();

            //  Ctrlを押しながら実行した場合、別アプリケーションで実行
            if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down)
            {
                AlternateApplication.Execute(e.Args.Length > 0 ? e.Args[0] : "");
                Application.Current.Shutdown();
            }

            //  すでに起動しているプロセスに名前付きパイプを送信
            mutex = new Mutex(false, "GazoView.exe");
            if(!mutex.WaitOne(0, false))
            {
                PipeMessage.Send(e.Args);
                Application.Current.Shutdown();
            }

            //  引数から画像ファイル読み込み
            Item.ImageStore = new ImageStore();
            Item.ImageStore.SetItems(e.Args);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Item.Data.Setting.Serialize();
            mutex.Dispose();
        }
    }
}

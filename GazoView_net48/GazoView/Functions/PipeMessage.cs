using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.IO.Pipes;

namespace GazoView.Functions
{
    /// <summary>
    /// 同アプリケーション/他プロセスへ送信するパイプメッセージ用クラス
    /// </summary>
    public class PipeMessage
    {
        public string[] Items { get; set; }

        const string PIPEMESSAGE_FILENAME = "pipemessage.json";

        private static string pipemessage_FileName = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), PIPEMESSAGE_FILENAME);
        private const string PIPE_NAME = "PIPE_GAZOVIEW_MULTIPROCESS";
        private const int PIPE_CONNECT_TIMEOUT = 3000;
        private const string EXIT_MESSAGE = "gazoview_eixt_message";
        private const string SEND_MESSAGE = "gazoview_invoke_message";

        /// <summary>
        /// 他プロセスの待ち受け
        /// </summary>
        public static async Task Start(MainWindow mainWindow)
        {
            while (true)
            {
                using (var ps = new NamedPipeServerStream(PIPE_NAME, PipeDirection.In))
                {
                    await ps.WaitForConnectionAsync();
                    if (ps.IsConnected)
                    {
                        using (var sr = new StreamReader(ps))
                        {
                            string message = await sr.ReadToEndAsync();
                            if (message == EXIT_MESSAGE)
                            {
                                break;
                            }
                            UpdateImageForMainWindow(mainWindow);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 待ち受けを終了
        /// </summary>
        public static void Close()
        {
            using (NamedPipeClientStream ps = new NamedPipeClientStream(".", PIPE_NAME, PipeDirection.Out))
            {
                try
                {
                    ps.Connect(PIPE_CONNECT_TIMEOUT);
                    using (StreamWriter sw = new StreamWriter(ps))
                    {
                        sw.Write(EXIT_MESSAGE);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 他プロセスへメッセージを送信
        /// </summary>
        /// <param name="items"></param>
        public static void Send(string[] items)
        {
            PipeMessage message = new PipeMessage()
            {
                Items = items,
            };
            using (var sw = new StreamWriter(pipemessage_FileName, false, Encoding.UTF8))
            {
                sw.Write(Newtonsoft.Json.JsonConvert.SerializeObject(message));
            }

            using (NamedPipeClientStream ps = new NamedPipeClientStream(".", PIPE_NAME, PipeDirection.Out))
            {
                try
                {
                    ps.Connect(PIPE_CONNECT_TIMEOUT);
                    using (StreamWriter sw = new StreamWriter(ps))
                    {
                        sw.Write(SEND_MESSAGE);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// パイプ受け取り後の処理
        /// </summary>
        /// <param name="mainWindow"></param>
        private static void UpdateImageForMainWindow(MainWindow mainWindow)
        {
            try
            {
                using (var sr = new StreamReader(pipemessage_FileName, Encoding.UTF8))
                {
                    PipeMessage message =
                       Newtonsoft.Json.JsonConvert.DeserializeObject<PipeMessage>(sr.ReadToEnd());
                    if (message.Items == null || message.Items.Length == 0)
                    {
                        mainWindow.Activate();
                        mainWindow.Focus();
                    }
                    else
                    {
                        //  何も選択せずにGazoView.exeを実行した場合は、すでに起動しているウィンドウを最前面に。
                        Item.ImageStore.SetItems(message.Items);
                        mainWindow.UpdateImage(0);
                    }
                }

                File.Delete(pipemessage_FileName);
            }
            catch { }
        }
    }
}

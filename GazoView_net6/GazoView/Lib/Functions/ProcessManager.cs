using GazoView.Lib.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GazoView.Lib.Functions
{
    internal class ProcessManager
    {
        const int PIPE_CONNECT_TIMEOUT = 3000;

        public Session Session { get; private set; }

        public bool Enabled { get; private set; }

        /// <summary>
        /// コンストラクタですでに実行済みプロセスのセッション情報チェック
        /// </summary>
        /// <param name="args"></param>
        public ProcessManager(string[] args, bool isMultiWindow)
        {
            this.Session = new Session();

            List<Session> list = null;
            try
            {
                list = JsonSerializer.Deserialize<List<Session>>(
                    File.ReadAllText(Item.FilePath.SessionFile));
            }
            catch
            {
                list = new();
            }

            if (list.Count > 0 &&
                !isMultiWindow &&
                Process.GetProcesses().Count(x => x.ProcessName == Item.ProcessName) > 1)
            {
                //  他プロセスに引数(画像ファイルパス)を送る
                SendToActiveProcess(args, list[0].Id);
                return;
            }

            //  起動開始
            this.Enabled = true;
            list.Add(Session);
            File.WriteAllText(Item.FilePath.SessionFile, JsonSerializer.Serialize(list));
            this.PipeReceive().ConfigureAwait(false);
        }

        /// <summary>
        /// 起動済みプロセスへパイプを送信
        /// </summary>
        /// <param name="args"></param>
        /// <param name="targetId"></param>
        private void SendToActiveProcess(string[] args, int targetId)
        {
            var message = new PipeMessage()
            {
                Command = PipeMessage.CommandType.OpenImage,
                Items = args,
                Date = Session.Date,
            };
            string tempPipeMessageFile = Item.FilePath.GetTempPipeMessageFile(Session.Id);
            message.Save(tempPipeMessageFile);

            PipeSend(targetId, tempPipeMessageFile).Wait();
        }

        /// <summary>
        /// パイプ送信用メソッド
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task PipeSend(int targetId, string path)
        {
            using (var ps = new NamedPipeClientStream(".", $"PIPE_GAZOVIEW_{targetId}", PipeDirection.Out))
            {
                try
                {
                    ps.Connect(PIPE_CONNECT_TIMEOUT);
                    using (var sw = new StreamWriter(ps))
                    {
                        await sw.WriteAsync(path);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// パイプ受信用メソッド
        /// </summary>
        /// <returns></returns>
        public async Task PipeReceive()
        {
            while (true)
            {
                using (var ps = new NamedPipeServerStream($"PIPE_GAZOVIEW_{Session.Id}", PipeDirection.In))
                {
                    await ps.WaitForConnectionAsync();
                    if (ps.IsConnected)
                    {
                        using (var sr = new StreamReader(ps))
                        {
                            string tempPipeMessageFile = await sr.ReadToEndAsync();
                            var message = JsonSerializer.Deserialize<PipeMessage>(File.ReadAllText(tempPipeMessageFile));
                            switch (message.Command)
                            {
                                case PipeMessage.CommandType.OpenImage:
                                    Item.BindingParam.Images.UpdateFileList(message.Items);

                                    //  最前面にする (最前面にした後、一度ウィンドウを選択しないと後ろに回らないので、あまり良い手法ではない)
                                    Item.MainBase.Activate();
                                    Item.MainBase.Topmost = true;
                                    Item.MainBase.Topmost = false;
                                    Item.MainBase.Focus();

                                    File.Delete(tempPipeMessageFile);
                                    break;
                                case PipeMessage.CommandType.Shutdown:
                                    //  使用するかどうかは未定。
                                    //  この情報を受け取ったらアプリケーションを終了
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public void MoveToFront()
        {
            List<Session> list = null;
            try
            {
                list = JsonSerializer.Deserialize<List<Session>>(
                    File.ReadAllText(Item.FilePath.SessionFile));
            }
            catch
            {
                list = new();
            }

            if (list.Count > 1 && list[0].Id != Session.Id)
            {
                list.RemoveAll(x => x.Id == Session.Id);
                list.Insert(0, Session);
                File.WriteAllText(Item.FilePath.SessionFile, JsonSerializer.Serialize(list));
            }
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Close()
        {
            if (Enabled)
            {
                List<Session> list = null;
                try
                {
                    list = JsonSerializer.Deserialize<List<Session>>(
                        File.ReadAllText(Item.FilePath.SessionFile));
                }
                catch
                {
                    list = new();
                }

                list.RemoveAll(x => x.Id == Session.Id);
                File.WriteAllText(Item.FilePath.SessionFile, JsonSerializer.Serialize(list));
            }
        }
    }
}

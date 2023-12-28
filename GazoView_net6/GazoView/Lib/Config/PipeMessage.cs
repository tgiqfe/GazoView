using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace GazoView.Lib.Config
{
    /// <summary>
    /// 他プロセスへ情報共有する為のデータ構造
    /// このデータを保存したファイルのパスをパイプ送信。
    /// </summary>
    internal class PipeMessage
    {
        public enum CommandType
        {
            None,       //  初期値。何もしない
            Shutdown,   //  アプリケーション終了。実装未定
            OpenImage,  //  受け取った画像ファイルを開く。
        }

        public CommandType Command { get; set; }

        public string[] Items { get; set; }

        public DateTime Date { get; set; }

        public void Save(string path)
        {
            string output = path;
            string json = JsonSerializer.Serialize(this);
            File.WriteAllText(output, json);
        }

        public static PipeMessage Load(string path)
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<PipeMessage>(json);
        }
    }
}

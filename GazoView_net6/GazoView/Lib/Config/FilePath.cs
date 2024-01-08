using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Config
{
    internal class FilePath
    {
        public string WorkingDirectory { get; set; }

        /// <summary>
        /// セッションファイル private
        /// </summary>
        private string _sessionFile = null;

        /// <summary>
        /// セッションファイル
        /// </summary>
        public string SessionFile
        {
            get
            {
                _sessionFile ??= Path.Combine(WorkingDirectory, "Session.json");
                return _sessionFile;
            }
        }

        /// <summary>
        /// パイプ送信時に一時的に使用するパイプメッセージファイル
        /// プロセスIDごとに変わる為、メソッドで実装
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTempPipeMessageFile(int id)
        {
            return Path.Combine(WorkingDirectory, $"message_{id}.json");
        }

        /// <summary>
        /// 設定ファイル private
        /// </summary>
        private string _settingFile = null;

        /// <summary>
        /// 設定ファイル
        /// </summary>
        public string SettingFile
        {
            get
            {
                _settingFile ??= Path.Combine(WorkingDirectory, "Setting.json");
                return _settingFile;
            }
        }

        public FilePath()
        {
            WorkingDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            if (!Directory.Exists(WorkingDirectory))
            {
                Directory.CreateDirectory(WorkingDirectory);
            }
        }
    }
}

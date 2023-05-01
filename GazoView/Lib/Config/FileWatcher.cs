using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GazoView.Lib.Config
{
    internal class FileWatcher
    {
        private FileSystemWatcher _watcher = null;

        /// <summary>
        /// ファイル変更(追加/削除)されたことを検知した場合にtrue化する
        /// </summary>
        private bool _onEvent = false;

        /// <summary>
        /// 監視中であるかどうか
        /// </summary>
        private bool _watching = false;

        /// <summary>
        /// ポーリングスタート済みかどうか
        /// </summary>
        private bool _started = false;

        public void StarFileListUpdate()
        {
            if (Item.BindingParam.Images != null)
            {
                _watcher = new();
                _watcher.Path = Item.BindingParam.Images.Parent;
                _watcher.NotifyFilter = NotifyFilters.FileName;
                _watcher.Created += (souce, e) => { _onEvent = true; };
                _watcher.Deleted += (souce, e) => { _onEvent = true; };
                _watcher.EnableRaisingEvents = true;

                if (!_started)
                {
                    Polling().ConfigureAwait(false);
                    _started = true;
                }
            }
            _watching = true;
        }

        public void StopFileListUpdate()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
            }
            _watching = false;
        }

        private async Task Polling()
        {
            int interval = Item.BindingParam.Setting.FileListUpdateInterval;
            if (interval < 1500)
            {
                Item.BindingParam.Setting.FileListUpdateInterval = 1500;
                interval = 1500;
            }
            while (true)
            {
                await Task.Delay(interval);
                if (_watching && _onEvent)
                {
                    Item.BindingParam.Images.UpdateFileList();
                    _onEvent = false;
                }
            }
        }
    }
}

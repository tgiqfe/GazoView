﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace GazoView.Lib.Conf
{
    /// <summary>
    /// 削除したファイルを一時保管/管理するクラス
    /// </summary>
    internal class DeletedStore
    {
        const string CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-+[]()@.,";
        const int NAME_LENGTH = 16;

        public string DeletedPath { get; private set; }
        public string Serial { get; private set; }
        public List<string> DeletedList { get; private set; }

        public DeletedStore()
        {
            var random = new Random();
            this.Serial = new string(
                Enumerable.Repeat(CHARACTERS, NAME_LENGTH).
                    Select(x => x[random.Next(x.Length)]).
                    ToArray());
            this.DeletedPath = Path.Combine(
                Environment.GetEnvironmentVariable("TEMP"),
                "GazoViewTemp",
                this.Serial);
            Directory.CreateDirectory(this.DeletedPath);
            this.DeletedList = new();
        }

        /// <summary>
        /// 削除後一時保管フォルダーへコピー
        /// </summary>
        /// <param name="imageFilePath"></param>
        public void CopyToDeletedStore(string imageFilePath)
        {
            var fileName = Path.GetFileName(imageFilePath);
            var dstPath = Path.Combine(this.DeletedPath, fileName);
            File.Copy(imageFilePath, dstPath);
            this.DeletedList.Add(dstPath);
        }

        /// <summary>
        /// 一時保管フォルダーから復元
        /// </summary>
        /// <param name="imageFileParent"></param>
        public void RestoreFromDeletedStore(string imageFileParent)
        {
            if (DeletedList?.Count > 0)
            {
                var fileName = Path.GetFileName(this.DeletedList.Last());
                var srcPath = Path.Combine(this.DeletedPath, fileName);
                var dstPath = Path.Combine(imageFileParent, fileName);
                File.Move(srcPath, dstPath);
                DeletedList.RemoveAt(DeletedList.Count - 1);
            }
        }

        /// <summary>
        /// 終了時処理
        /// ※アプリケーション終了時に実行。
        /// </summary>
        public void Close()
        {
            if (Directory.Exists(this.DeletedPath))
            {
                Directory.Delete(this.DeletedPath, true);
            }
        }
    }
}

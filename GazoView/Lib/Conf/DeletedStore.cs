using GazoView.Lib.Functions;
using System;
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
        const string CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-+[]@";
        const int NAME_LENGTH = 16;

        public string DeletedPath { get; private set; }
        public string Serial { get; private set; }
        public List<string> DeletedList { get; private set; }


        private List<DeletedItem> DeletedList2 { get; set; }

        private class DeletedItem
        {
            public string TrueName { get; set; }
            public string ManagedName { get; set; }
        }



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
            this.DeletedList2 = new();
        }

        /// <summary>
        /// 削除後一時保管フォルダーへコピー
        /// </summary>
        /// <param name="imageFilePath"></param>
        public void CopyToDeletedStore(string imageFilePath)
        {
            /*
            //  一時保管フォルダーへコピーする際、名前が重複しない前提の処理
            var fileName = Path.GetFileName(imageFilePath);
            var dstPath = Path.Combine(this.DeletedPath, fileName);
            File.Copy(imageFilePath, dstPath);
            this.DeletedList.Add(dstPath);
            */
            var fileName = Path.GetFileName(imageFilePath);
            var deletedItem = new DeletedItem()
            {
                TrueName = fileName,
                ManagedName = FilePaths.Deduplicate(DeletedPath, fileName),
            };
            File.Copy(imageFilePath, Path.Combine(DeletedPath, deletedItem.ManagedName));
            this.DeletedList2.Add(deletedItem);
        }

        /// <summary>
        /// 一時保管フォルダーから復元
        /// </summary>
        /// <param name="imageFileParent"></param>
        public void RestoreFromDeletedStore(string imageFileParent)
        {
            /*
            //  一時保管フォルダーへコピーする際、名前が重複しない前提の処理
            if (DeletedList?.Count > 0)
            {
                var fileName = Path.GetFileName(this.DeletedList.Last());
                var srcPath = Path.Combine(this.DeletedPath, fileName);
                var dstPath = Path.Combine(imageFileParent, fileName);
                File.Move(srcPath, dstPath);
                DeletedList.RemoveAt(DeletedList.Count - 1);
            }
            */
            if (DeletedList2?.Count > 0)
            {
                var deletedItem = this.DeletedList2.Last();
                var srcPath = Path.Combine(this.DeletedPath, deletedItem.ManagedName);
                var dstPath = Path.Combine(imageFileParent, deletedItem.TrueName);
                File.Move(srcPath, dstPath);
                DeletedList2.RemoveAt(DeletedList2.Count - 1);
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

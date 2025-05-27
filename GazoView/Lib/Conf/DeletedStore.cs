using GazoView.Lib.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Conf
{
    internal class DeletedStore
    {
        const string CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-+[]@";
        const int NAME_LENGTH = 16;

        public string DeletedPath { get; private set; }
        public string Serial { get; private set; }
        public List<DeletedItem> DeletedList { get; set; }

        public class DeletedItem
        {
            public string TrueName { get; set; }
            public string ManagedName { get; set; }
        }

        public string RestorableFileName
        {
            get { return DeletedList.Last().TrueName; }
        }

        public string RestoredFilePath { get; private set; }

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

        public void CopyToDeletedStore(string imageFilePath)
        {
            var fileName = Path.GetFileName(imageFilePath);
            var deletedItem = new DeletedItem
            {
                TrueName = fileName,
                ManagedName = FilePaths.Deduplicate(DeletedPath, fileName),
            };
            File.Copy(imageFilePath, Path.Combine(DeletedPath, deletedItem.ManagedName));
            this.DeletedList.Add(deletedItem);
        }

        public void RestoreFromDeletedStore(string imageFileParent)
        {
            if (DeletedList?.Count > 0)
            {
                var deletedItem = this.DeletedList.Last();
                var srcPath = Path.Combine(this.DeletedPath, deletedItem.ManagedName);
                var dstPath = Path.Combine(imageFileParent, deletedItem.TrueName);
                File.Move(srcPath, dstPath);
                DeletedList.RemoveAt(DeletedList.Count - 1);
                RestoredFilePath = dstPath;
            }
        }

        public void Close()
        {
            if (Directory.Exists(this.DeletedPath))
            {
                Directory.Delete(this.DeletedPath, true);
            }
        }
    }
}

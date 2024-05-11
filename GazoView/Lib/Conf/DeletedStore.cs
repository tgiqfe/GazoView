using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace GazoView.Lib.Conf
{
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

        public void ToDeletedStore(string path)
        {
            var fileName = Path.GetFileName(path);
            var destPath = Path.Combine(this.DeletedPath, fileName);
            File.Copy(path, destPath);
            this.DeletedList.Add(destPath);
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

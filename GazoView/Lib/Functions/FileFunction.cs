using System.IO;
using System.Security.Cryptography;

namespace GazoView.Lib.Functions
{
    internal class FileFunction
    {
        public static string GetFileSize(long FileSize)
        {
            return FileSize switch
            {
                long size when size < 1024 => $"{size} Byte",
                long size when size < 1024 * 1024 => $"{size / 1024D:F2} KB",
                long size when size < 1024 * 1024 * 1024 => $"{size / (1024D * 1024):F2} MB",
                long size when size < 1024L * 1024 * 1024 * 1024 => $"{size / (1024D * 1024 * 1024):F2} GB",
                long size when size < 1024L * 1024 * 1024 * 1024 * 1024 => $"{size / (1024D * 1024 * 1024 * 1024):F2} TB",
                _ => ""
            };
        }

        public static string GetHash(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(fs);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }
    }
}

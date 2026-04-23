using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Security.Cryptography;

namespace GazoView.Lib.Functions
{
    internal class FileFunction
    {
        /// <summary>
        /// Get file size string.
        /// </summary>
        /// <param name="FileSize"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get MD5 hash string.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Rename file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newName"></param>
        public static void RenameFile(string path, string newName)
        {
            FileSystem.RenameFile(path, newName);
        }

        /// <summary>
        /// Delete file, to recylebin.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="toRecycleBin"></param>
        public static void DeleteFile(string path, bool toRecycleBin = true)
        {
            if (path.StartsWith("\\\\"))
            {
                File.Delete(path);
            }
            else
            {
                FileSystem.DeleteFile(
                    path,
                    UIOption.OnlyErrorDialogs,
                    toRecycleBin ? RecycleOption.SendToRecycleBin : RecycleOption.DeletePermanently);
            }
        }

        /// <summary>
        /// Move file.
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        public static void MoveFile(string sourcePath, string destinationPath)
        {
            FileSystem.MoveFile(sourcePath, destinationPath);
        }

        /// <summary>
        /// Get safe (non-conflicting) file name.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetSafeNamePth(string path)
        {
            if (File.Exists(path)) return path;

            var parent = Path.GetDirectoryName(path);
            var baseName = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);

            for (int i = 1; i < 1000; i++)
            {
                var tempPath = Path.Combine(parent, $"{baseName}_{i}{extension}");
                if (File.Exists(tempPath))
                {
                    continue;
                }
                else
                {
                    return tempPath;
                }
            }
            throw new Exception("Failed to generate a safe file name.");
        }
    }
}

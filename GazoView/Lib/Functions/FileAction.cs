using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GazoView.Lib.Functions
{
    internal class FileAction
    {
        /// <summary>
        /// 重複していないファイルパスを返す
        /// </summary>
        /// <param name="path">保存したい先のパス</param>
        /// <returns>名前重複していないパス</returns>
        public static string CreateSafePath(string path)
        {
            if (!File.Exists(path)) { return path; }

            string extension = Path.GetExtension(path);
            string baseName = Path.GetFileNameWithoutExtension(path);
            string parent = Path.GetDirectoryName(path);
            int length = baseName.Length;
            Regex pattern_num = new Regex(@"_\d+$");

            var suffixes = Directory.GetFiles(parent).
                Select(x => Path.GetFileNameWithoutExtension(x)).
                Where(x => x.StartsWith(baseName)).
                Select(x => x.Substring(length)).
                Where(x => pattern_num.IsMatch(x)).
                Select(x => int.Parse(x.TrimStart('_')));

            int max = suffixes.Count() > 0 ? suffixes.Max() : 0;

            return $"{parent}\\{baseName}_{max + 1}{extension}";
        }

        /// <summary>
        /// 指定したファイルの場所をエクスプローラで開く
        /// </summary>
        /// <param name="path"></param>
        public static void OpenExplorerForFile(string path)
        {
            using (var proc = new Process())
            {
                proc.StartInfo.FileName = "explorer.exe";
                proc.StartInfo.Arguments = $"/select, \"{path}\"";
                proc.Start();
            }
        }

        /// <summary>
        /// 外部アプリでファイルを開く
        /// </summary>
        /// <param name="path"></param>
        public static void ExecuteAltenateApp(string appPath, string path)
        {
            if (string.IsNullOrEmpty(appPath)) { return; }

            using (var proc = new Process())
            {
                proc.StartInfo.FileName = appPath;
                proc.StartInfo.Arguments = $"\"{path}\"";
                proc.Start();
            }
        }
    }
}

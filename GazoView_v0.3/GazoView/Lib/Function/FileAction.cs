using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GazoView.Lib.Function
{
    internal class FileAction
    {
        /// <summary>
        /// 重複していないファイルを返す
        /// </summary>
        /// <param name="path">元ファイルのパス</param>
        /// <returns>名前重複していないパス</returns>
        public static string CreateSafePath(string path)
        {
            if (!File.Exists(path)) return null;

            string parent = Path.GetDirectoryName(path);
            string baseName = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            int length = baseName.Length;
            Regex pattern = new Regex(@"_\d+$");

            var suffixes = Directory.GetFiles(parent).
                Select(x => Path.GetFileNameWithoutExtension(x)).
                Where(x => x.StartsWith(baseName)).
                Select(x => x.Substring(length)).
                Where(x => pattern.IsMatch(x)).
                Select(x => int.Parse(x.TrimStart('_')));
            int max = suffixes.Count() > 0 ? suffixes.Max() : 0;

            return Path.Combine(parent, $"{baseName}_{max + 1}{extension}");
        }
    }
}

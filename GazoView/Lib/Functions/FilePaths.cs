using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Functions
{
    internal class FilePaths
    {
        /// <summary>
        /// 対象のパスのファイルがすでに存在する場合、別名を取得
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Deduplicate(string path)
        {
            if (!File.Exists(path)) return path;

            int maxCount = 999;
            string parent = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            for (int i = 1; i < maxCount; i++)
            {
                string tempPath = Path.Combine(parent, $"{fileName}_{i}{extension}");
                if (!File.Exists(tempPath))
                {
                    return tempPath;
                }
            }
            return null;
        }

        /// <summary>
        /// 対象のパスのファイルがすでに存在する場合、別名を取得
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Deduplicate(string parent, string name)
        {
            return Deduplicate(Path.Combine(parent, name));
        }
    }
}

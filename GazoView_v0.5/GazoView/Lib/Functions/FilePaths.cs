using System.IO;

namespace GazoView.Lib.Functions
{
    internal class FilePaths
    {
        const int DEDUPLICATE_MAX_COUNT = 9999;

        public static string Deduplicate(string path)
        {
            if (!File.Exists(path)) return path;
            string parent = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            for (int i = 1; i <= DEDUPLICATE_MAX_COUNT; i++)
            {
                string tempPath = Path.Combine(parent, $"{fileName}_{i}{extension}");
                if (!File.Exists(tempPath))
                {
                    return tempPath;
                }
            }
            return null;
        }

        public static string Deduplicate(string parent, string name)
        {
            return Deduplicate(Path.Combine(parent, name));
        }
    }
}

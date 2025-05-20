using GazoView.Lib.Conf;
using System.Collections.Specialized;
using System.IO;

namespace GazoView.Lib.Functions
{
    internal class FileFunction
    {
        public static void ToggleStarFile(Images images)
        {
            if (images.Length > 0)
            {
                var newPath = images.Current.IsStar ?
                    Path.Combine(
                        images.Current.Parent,
                        Path.GetFileNameWithoutExtension(images.Current.FileName).TrimEnd('★') + images.Current.FileExtension) :
                    Path.Combine(
                        images.Current.Parent,
                        Path.GetFileNameWithoutExtension(images.Current.FileName) + "★" + images.Current.FileExtension);
                File.Move(images.Current.FilePath, newPath);

                images.ReloadFiles(newPath);
            }
        }

        public static void CopyImageFile(Images images, bool isText = false)
        {
            if (images.Length > 0)
            {
                if (isText)
                {
                    string text = images.Current.FilePath;
                    System.Windows.Clipboard.SetText(text);
                }
                else
                {
                    var targets = new StringCollection();
                    targets.Add(images.Current.FilePath);
                    System.Windows.Clipboard.SetFileDropList(targets);
                }
            }
        }
    }
}

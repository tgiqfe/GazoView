using GazoView.Lib.Conf;
using System.Collections.Specialized;
using System.IO;
using System.Windows;

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

        public static void DeleteImageFile()
        {
            if (Item.BindingParam.Images.FileList.Count > 0)
            {
                var ret = MessageBox.Show($"Delete?\n{Item.BindingParam.Images.Current.FileName}",
                    Item.ProcessName,
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Information,
                    MessageBoxResult.OK);
                if (ret == MessageBoxResult.OK)
                {
                    Item.DeletedStore ??= new();
                    Item.DeletedStore.CopyToDeletedStore(Item.BindingParam.Images.Current.FilePath);
                    Item.BindingParam.Images.DeleteCurrentImageFile();
                }
            }
        }

        public static void RestoreImageFile()
        {
            if (Item.DeletedStore?.DeletedList?.Count > 0)
            {
                var ret = MessageBox.Show($"Restore?\n{Item.DeletedStore.RestorableFileName}",
                    Item.ProcessName,
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question,
                    MessageBoxResult.OK);
                if(ret == MessageBoxResult.OK)
                {
                    Item.DeletedStore.RestoreFromDeletedStore(Item.BindingParam.Images.Current.Parent);
                    Item.BindingParam.Images.ReloadFiles(Item.DeletedStore.RestoredFilePath);
                }
            }
        }
    }
}

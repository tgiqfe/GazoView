using GazoView.Lib.Functions;
using GazoView.Lib.Panel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GazoView.Lib
{
    public class MessageDialog : IDisposable
    {
        private MessageDialogWindow _messageDialogWindow;
        public bool IsVisible { get; set; }

        const string CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-+[]@";
        const int NAME_LENGTH = 16;

        private string _storeDirectory = null;
        private string _serial = null;
        private List<DeletedItem> DeletedList = null;
        private class DeletedItem
        {
            public string TrueName { get; set; }
            public string ManagedName { get; set; }
            public DateTime LastWriteTime { get; set; }
        }

        public MessageDialog()
        {
            var random = new Random();
            _serial = new string(
                Enumerable.Repeat(CHARACTERS, NAME_LENGTH).
                    Select(s => s[random.Next(s.Length)]).
                    ToArray());
            _storeDirectory = Path.Combine(
                Environment.GetEnvironmentVariable("TEMP"),
                Item.ProcessName,
                this._serial);
            this.DeletedList = new();
        }

        #region Show/Hide window.

        /// <summary>
        /// Show message window for delete.
        /// </summary>
        public void ShowDeleteWindow()
        {
            _messageDialogWindow = new();
            _messageDialogWindow.TextBlockAction.Text = "Delete?";
            _messageDialogWindow.TextBlockFilePath.Text = Item.BindingParam.Images.Current.FilePath;
            _messageDialogWindow.TextBlockFileName.Text = Item.BindingParam.Images.Current.FileName;
            _messageDialogWindow.TextBlockFileExtension.Text = Item.BindingParam.Images.Current.FileExtension;
            _messageDialogWindow.TextBlockImageSize.Text = Item.BindingParam.Images.Current.Resolution;
            _messageDialogWindow.TextBlockFileSize.Text = Item.BindingParam.Images.Current.Size;
            _messageDialogWindow.TextBlockTimeStamp.Text = Item.BindingParam.Images.Current.LastWriteTime;
            _messageDialogWindow.TargetImage.Source = Item.BindingParam.Images.Current.Source;
            _messageDialogWindow.Owner = Item.MainWindow;
            _messageDialogWindow.Show();

            _messageDialogWindow.ButtonOK.Click += (s, e) =>
            {
                //this.DeleteImageFile(Item.BindingParam.Images.Current);

                if (!Directory.Exists(_storeDirectory))
                {
                    Directory.CreateDirectory(_storeDirectory);
                }
                var imageItem = Item.BindingParam.Images.Current;
                string managedName = Guid.NewGuid().ToString() + imageItem.FileExtension;
                string managedPath = Path.Combine(_storeDirectory, managedName);
                this.DeletedList.Add(new DeletedItem()
                {
                    TrueName = imageItem.FileName,
                    ManagedName = managedName,
                    LastWriteTime = imageItem.LastWriteTimeRaw
                });
                File.Copy(imageItem.FilePath, managedPath);
                new FileInfo(managedPath).LastWriteTime = imageItem.LastWriteTimeRaw;

                Item.BindingParam.Images.DeleteImageFile();
                HideWindow();
            };
            _messageDialogWindow.ButtonCancel.Click += (s, e) =>
            {
                HideWindow();
            };
            _messageDialogWindow.Show();
            this.IsVisible = true;
        }

        /// <summary>
        /// Show message window for restore.
        /// </summary>
        public void ShowRestoreWindow()
        {
            if (this.DeletedList.Count == 0) return;

            var restorableFile = this.DeletedList.Last();
            var managedPath = Path.Combine(_storeDirectory, restorableFile.ManagedName);
            var source = new ImageItem(managedPath);

            _messageDialogWindow = new();
            _messageDialogWindow.TextBlockAction.Text = "Restore?";
            _messageDialogWindow.TextBlockFilePath.Text = Path.Combine(Item.BindingParam.Images.Current.Parent, restorableFile.TrueName);
            _messageDialogWindow.TextBlockFileName.Text = restorableFile.TrueName;
            _messageDialogWindow.TextBlockFileExtension.Text = Path.GetExtension(restorableFile.TrueName);
            _messageDialogWindow.TextBlockImageSize.Text = source.Resolution;
            _messageDialogWindow.TextBlockFileSize.Text = source.Size;
            _messageDialogWindow.TextBlockTimeStamp.Text = restorableFile.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");
            _messageDialogWindow.TargetImage.Source = source.Source;
            _messageDialogWindow.Owner = Item.MainWindow;

            _messageDialogWindow.ButtonOK.Click += (s, e) =>
            {
                string managedPath = Path.Combine(_storeDirectory, DeletedList.Last().ManagedName);
                Item.BindingParam.Images.MoveInImageFile(
                    managedPath,
                    DeletedList.Last().TrueName);
                this.DeletedList.RemoveAt(DeletedList.Count - 1);
                HideWindow();
            };
            _messageDialogWindow.ButtonCancel.Click += (s, e) =>
            {
                HideWindow();
            };
            _messageDialogWindow.Show();
            this.IsVisible = true;
        }

        /// <summary>
        /// Hide message window.
        /// </summary>
        public void HideWindow()
        {
            _messageDialogWindow.Hide();
            _messageDialogWindow.Close();
            this.IsVisible = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                _messageDialogWindow = null;
            });
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_storeDirectory != null && Directory.Exists(_storeDirectory))
            {
                Directory.Delete(_storeDirectory, true);
            }
        }

        #endregion
    }
}

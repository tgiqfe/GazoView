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
    public class DeleteMessage : IDisposable
    {
        private DeleteMessageWindow _deleteMessageWindow;
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

        public DeleteMessage()
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

        public void CopyToDeletedStore(string truePath)
        {
            if (!Directory.Exists(_storeDirectory))
            {
                Directory.CreateDirectory(_storeDirectory);
            }
            string managedName = Guid.NewGuid().ToString() + Path.GetExtension(truePath);
            DateTime lastWriteTime = File.GetLastWriteTime(truePath);
            this.DeletedList.Add(new DeletedItem
            {
                TrueName = Path.GetFileName(truePath),
                ManagedName = managedName,
                LastWriteTime = lastWriteTime
            });
            var managedPath = Path.Combine(_storeDirectory, managedName);

            File.Copy(truePath, managedPath);
            new FileInfo(managedPath).LastWriteTime = lastWriteTime;
        }

        public void RestoreFromDeletedStore()
        {
            if (this.DeletedList.Count == 0)
            {
                return;
            }
            string managedPath = Path.Combine(_storeDirectory, DeletedList.Last().ManagedName);
            string truePath = Path.Combine(Item.BindingParam.Images.Current.Parent, this.DeletedList.Last().TrueName);
            if (File.Exists(truePath))
            {
                // If a file with the same name already exists, append a number to the file name.
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(truePath);
                string extension = Path.GetExtension(truePath);
                int copyIndex = 1;
                while (File.Exists(truePath))
                {
                    truePath = Path.Combine(Item.BindingParam.Images.Current.Parent, $"{fileNameWithoutExtension} ({copyIndex}){extension}");
                    copyIndex++;
                }
            }
            File.Move(managedPath, truePath);
            new FileInfo(truePath).LastWriteTime = this.DeletedList.Last().LastWriteTime;
            this.DeletedList.RemoveAt(DeletedList.Count - 1);
        }

        #region Show/Hide window.

        /// <summary>
        /// Show message window for delete.
        /// </summary>
        public void ShowDeleteWindow()
        {
            _deleteMessageWindow = new();
            _deleteMessageWindow.TextBlockAction.Text = "Delete?";
            _deleteMessageWindow.TextBlockFilePath.Text = Item.BindingParam.Images.Current.FilePath;
            _deleteMessageWindow.TextBlockFileName.Text = Item.BindingParam.Images.Current.FileName;
            _deleteMessageWindow.TextBlockFileExtension.Text = Item.BindingParam.Images.Current.FileExtension;
            _deleteMessageWindow.TextBlockImageSize.Text = Item.BindingParam.Images.Current.Resolution;
            _deleteMessageWindow.TextBlockFileSize.Text = Item.BindingParam.Images.Current.Size;
            _deleteMessageWindow.TextBlockTimeStamp.Text = Item.BindingParam.Images.Current.LastWriteTime;
            _deleteMessageWindow.TargetImage.Source = Item.BindingParam.Images.Current.Source;
            _deleteMessageWindow.Owner = Item.MainWindow;
            _deleteMessageWindow.Show();

            _deleteMessageWindow.ButtonOK.Click += (s, e) =>
            {
                CopyToDeletedStore(Item.BindingParam.Images.Current.FilePath);
                Item.BindingParam.Images.DeleteImageFile();
                HideWindow();
            };
            _deleteMessageWindow.ButtonCancel.Click += (s, e) =>
            {
                HideWindow();
            };
            _deleteMessageWindow.Show();
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

            _deleteMessageWindow = new();
            _deleteMessageWindow.TextBlockAction.Text = "Restore?";
            _deleteMessageWindow.TextBlockFilePath.Text = Path.Combine(Item.BindingParam.Images.Current.Parent, restorableFile.TrueName);
            _deleteMessageWindow.TextBlockFileName.Text = restorableFile.TrueName;
            _deleteMessageWindow.TextBlockFileExtension.Text = Path.GetExtension(restorableFile.TrueName);
            _deleteMessageWindow.TextBlockImageSize.Text = source.Resolution;
            _deleteMessageWindow.TextBlockFileSize.Text = source.Size;
            _deleteMessageWindow.TextBlockTimeStamp.Text = restorableFile.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");
            _deleteMessageWindow.TargetImage.Source = source.Source;
            _deleteMessageWindow.Owner = Item.MainWindow;

            _deleteMessageWindow.ButtonOK.Click += (s, e) =>
            {
                RestoreFromDeletedStore();
                //  ここがうまくいかない。
                Item.BindingParam.Images.ReloadAndJumpFile(restorableFile.TrueName);

                HideWindow();
            };
            _deleteMessageWindow.ButtonCancel.Click += (s, e) =>
            {
                HideWindow();
            };
            _deleteMessageWindow.Show();
            this.IsVisible = true;
        }

        /// <summary>
        /// Hide message window.
        /// </summary>
        public void HideWindow()
        {
            _deleteMessageWindow.Hide();
            _deleteMessageWindow.Close();
            this.IsVisible = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                _deleteMessageWindow = null;
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

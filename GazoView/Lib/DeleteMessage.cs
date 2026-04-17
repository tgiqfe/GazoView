using GazoView.Lib.Functions;
using GazoView.Lib.Panel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
        }
        private DeletedItem RestorableFile
        {
            get => DeletedList.Last();
        }

        private string RestoredFilePath = null;

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

        private void CopyToDeletedStore(string filePath)
        {
            if (!Directory.Exists(_storeDirectory))
            {
                Directory.CreateDirectory(_storeDirectory);
            }
            string managedName = Guid.NewGuid().ToString();
            File.Copy(filePath, Path.Combine(_storeDirectory, managedName));
            DeletedList.Add(new DeletedItem
            {
                TrueName = Path.GetFileName(filePath),
                ManagedName = managedName
            });
        }

        private void RestoreFromDeletedStore()
        {
            if (this.DeletedList.Count == 0)
            {
                return;
            }
            string sourcePath = Path.Combine(_storeDirectory, DeletedList.Last().ManagedName);
            File.Copy(sourcePath, RestoredFilePath);
            File.Delete(sourcePath);
            DeletedList.RemoveAt(DeletedList.Count - 1);
        }




        #region Show/Hide window.

        /// <summary>
        /// Show message window for delete.
        /// </summary>
        public void ShowDeleteWindow()
        {
            _deleteMessageWindow ??= new DeleteMessageWindow(
                "Delete?",
                Item.BindingParam.Images.Current.FilePath,
                Item.BindingParam.Images.Current.FileName,
                Item.BindingParam.Images.Current.FileExtension,
                Item.BindingParam.Images.Current.Resolution,
                Item.BindingParam.Images.Current.Size,
                Item.BindingParam.Images.Current.LastWriteTime,
                Item.BindingParam.Images.Current.Source);
            _deleteMessageWindow.Owner = Item.MainWindow;
            _deleteMessageWindow.Show();
            this.IsVisible = true;
        }

        /// <summary>
        /// Show message window for restore.
        /// </summary>
        public void ShowRestoreWindow()
        {
            if (this.DeletedList.Count == 0) return;

            var restoreFilePath = Path.Combine(_storeDirectory, this.RestorableFile.ManagedName);
            ImageSource imageSource = new BitmapImage(new Uri(restoreFilePath));

            _deleteMessageWindow ??= new DeleteMessageWindow(
                "Restore?",
                Path.Combine(Item.BindingParam.Images.Current.Parent, RestorableFile.TrueName),
                RestorableFile.TrueName,
                Path.GetExtension(RestorableFile.TrueName),
                $"{imageSource.Width} x {imageSource.Height}",
                FileFunction.GetFileSize(new FileInfo(restoreFilePath).Length),
                File.GetLastWriteTime(restoreFilePath).ToString("yyyy/MM/dd HH:mm:ss"),
                imageSource);
            _deleteMessageWindow.Owner = Item.MainWindow;
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
            _deleteMessageWindow = null;
            this.IsVisible = false;
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

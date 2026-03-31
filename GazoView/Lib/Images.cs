using GazoView.Lib.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace GazoView.Lib
{
    public class Images : INotifyPropertyChanged, IDisposable
    {
        private static readonly string[] _validExtensions = new string[]
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".bmp",
            ".tiff",
            ".tif",
            ".tif",
            ".webp",
            ".svg",
        };

        private FileSystemWatcher _watcher;
        private string _watchingDirectory;

        public ObservableCollection<string> FileList { get; private set; }

        public int Length { get { return this.FileList?.Count ?? 0; } }

        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                int length = this.Length;
                if (_index < 0)
                {
                    _index = length - 1;
                }
                else if (_index >= length)
                {
                    _index = 0;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Title
        {
            get
            {
                return string.Format("[{0}/{1}] {2}", this.Index + 1, this.Length, this.Current.FileName);
            }
        }

        public ImageItem Current { get; private set; }

        /// <summary>
        /// Constructor, load files.
        /// </summary>
        /// <param name="targets"></param>
        public Images(string[] targets)
        {
            LoadFiles(targets);
        }

        /// <summary>
        /// Load files from given paths, and filter by valid extensions.
        /// Target file or directory path.
        /// </summary>
        /// <param name="targets"></param>
        public void LoadFiles(string[] targets)
        {
            if (targets.Length > 0)
            {
                if (File.Exists(targets[0]))
                {
                    string parent = Path.GetDirectoryName(targets[0]);
                    var collection = Directory.GetFiles(parent).
                        Where(x => IsValidImageFile(x)).
                        OrderBy(x => x, new NaturalStringComparer());
                    this.FileList = new ObservableCollection<string>(collection);
                    this.Index = this.FileList.IndexOf(targets[0]);
                    ViewImage();
                    StartWatching(parent);
                }
                else if (Directory.Exists(targets[0]))
                {
                    var collection = Directory.GetFiles(targets[0]).
                        Where(x => IsValidImageFile(x)).
                        OrderBy(x => x, new NaturalStringComparer());
                    this.FileList = new ObservableCollection<string>(collection);
                    this.Index = 0;
                    ViewImage();
                    StartWatching(targets[0]);
                }
            }
        }

        public void ViewImage()
        {
            this.Current = new ImageItem(this.FileList[this.Index]);
            OnPropertyChanged(nameof(Current));
        }

        /// <summary>
        /// Start watching the directory for file changes.
        /// </summary>
        /// <param name="directory"></param>
        private void StartWatching(string directory)
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
            }

            _watchingDirectory = directory;
            _watcher = new FileSystemWatcher(directory)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
                Filter = "*.*",
                EnableRaisingEvents = true
            };

            _watcher.Created += OnFileChanged;
            _watcher.Changed += OnFileChanged;
            _watcher.Deleted += OnFileChanged;
            _watcher.Renamed += OnFileRenamed;
        }

        /// <summary>
        /// Handle file system changes.
        /// </summary>
        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (!IsValidImageFile(e.FullPath)) return;
            Application.Current?.Dispatcher.Invoke(() =>
            {
                RefreshFileList();
            });
        }

        /// <summary>
        /// Handle file rename events.
        /// </summary>
        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            if (!IsValidImageFile(e.FullPath) && !IsValidImageFile(e.OldFullPath)) return;

            Application.Current?.Dispatcher.Invoke(() =>
            {
                RefreshFileList();
            });
        }

        /// <summary>
        /// Check if the file is a valid image file.
        /// </summary>
        private bool IsValidImageFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return _validExtensions.Contains(extension);
        }

        /// <summary>
        /// Refresh the file list from the watching directory.
        /// </summary>
        private void RefreshFileList()
        {
            if (string.IsNullOrEmpty(_watchingDirectory) || !Directory.Exists(_watchingDirectory))
                return;

            string currentFile = this.FileList.Count > this.Index ? this.FileList[this.Index] : null;

            var collection = Directory.GetFiles(_watchingDirectory)
                .Where(x => IsValidImageFile(x))
                .OrderBy(x => x, new NaturalStringComparer())
                .ToList();

            this.FileList.Clear();
            foreach (var file in collection)
            {
                this.FileList.Add(file);
            }

            if (!string.IsNullOrEmpty(currentFile) && this.FileList.Contains(currentFile))
            {
                this.Index = this.FileList.IndexOf(currentFile);
            }
            else if (this.FileList.Count > 0)
            {
                this.Index = Math.Min(this.Index, this.FileList.Count - 1);
                ViewImage();
            }

            OnPropertyChanged(nameof(Length));
            OnPropertyChanged(nameof(Title));
        }



        #region Inotify change

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Created -= OnFileChanged;
                _watcher.Changed -= OnFileChanged;
                _watcher.Deleted -= OnFileChanged;
                _watcher.Renamed -= OnFileRenamed;
                _watcher.Dispose();
                _watcher = null;
            }
        }

        #endregion
    }
}

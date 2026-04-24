using GazoView.Lib.Functions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
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
        private string[] _fileListBeforeStop;

        /// <summary>
        /// File name list.
        /// image files in the same directory as the target file.
        /// </summary>
        public ObservableCollection<string> FileList { get; private set; }

        public int Length { get { return this.FileList?.Count ?? 0; } }

        private int _index = 0;
        private int _preview = -1;

        public int Index
        {
            get { return _index; }
            set
            {
                _preview = _index;
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
                    UpdateImage();
                    StartWatching(parent);
                }
                else if (Directory.Exists(targets[0]))
                {
                    var collection = Directory.GetFiles(targets[0]).
                        Where(x => IsValidImageFile(x)).
                        OrderBy(x => x, new NaturalStringComparer());
                    this.FileList = new ObservableCollection<string>(collection);
                    this.Index = 0;
                    UpdateImage();
                    StartWatching(targets[0]);
                }
            }
        }

        /// <summary>
        /// Check if the file is a valid image file.
        /// </summary>
        private bool IsValidImageFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return _validExtensions.Contains(extension);
        }

        public void UpdateImage()
        {
            this.Current = new ImageItem(this.FileList[this.Index]);
            OnPropertyChanged(nameof(Current));
            OnPropertyChanged(nameof(Title));
        }

        public void JumptoImage(string name)
        {
            var index = name.Contains("\\") ?
                this.FileList.IndexOf(name) :
                this.FileList.Select(x => Path.GetFileName(x)).ToList().IndexOf(name);
            if (index >= 0)
            {
                this.Index = index;
                UpdateImage();
            }
        }

        #region File wathcing start/stop/resume.

        /// <summary>
        /// Start watching the directory for file changes.
        /// </summary>
        /// <param name="directory"></param>
        public void StartWatching(string directory)
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
        /// Stop watching the directory for file changes.
        /// </summary>
        public void StopWatching()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _fileListBeforeStop = this.FileList?.ToArray();
            }
        }

        /// <summary>
        /// Resume watching the directory for file changes.
        /// </summary>
        public void ResumeWatching()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = true;
                if (_fileListBeforeStop != null && !string.IsNullOrEmpty(_watchingDirectory))
                {
                    var currentFiles = Directory.GetFiles(_watchingDirectory)
                        .Where(x => IsValidImageFile(x))
                        .OrderBy(x => x, new NaturalStringComparer())
                        .ToList();
                    if (!_fileListBeforeStop.SequenceEqual(currentFiles))
                    {
                        Application.Current?.Dispatcher.Invoke(() =>
                        {
                            RefreshFileList();
                        });
                    }
                    _fileListBeforeStop = null;
                }
            }
        }

        #endregion
        #region File watching event handlers.

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
        /// Refresh the file list from the watching directory.
        /// </summary>
        private void RefreshFileList()
        {
            if (string.IsNullOrEmpty(_watchingDirectory) || !Directory.Exists(_watchingDirectory)) return;

            string currentFile = this.FileList.Count > this.Index ? this.FileList[this.Index] : null;
            var collection = Directory.GetFiles(_watchingDirectory)
                .Where(x => IsValidImageFile(x))
                .OrderBy(x => x, new NaturalStringComparer())
                .ToList();

            this.FileList.Clear();
            collection.ForEach(x => this.FileList.Add(x));

            if (!string.IsNullOrEmpty(currentFile) && this.FileList.Contains(currentFile))
            {
                this.Index = this.FileList.IndexOf(currentFile);
            }
            else if (this.FileList.Count > 0)
            {
                this.Index = Math.Min(this.Index, this.FileList.Count - 1);
                UpdateImage();
            }

            OnPropertyChanged(nameof(Length));
            OnPropertyChanged(nameof(Title));
        }

        #endregion

        public void RenameImageFile(string newName)
        {
            //  stop watching to avoid multiple events triggered by renaming.
            StopWatching();

            FileFunction.RenameFile(this.Current.FilePath, newName);
            string currentPath = Path.Combine(Path.GetDirectoryName(this.Current.FilePath), newName);
            var collection = Directory.GetFiles(_watchingDirectory)
                .Where(x => IsValidImageFile(x))
                .OrderBy(x => x, new NaturalStringComparer())
                .ToList();
            this.FileList.Clear();
            collection.ForEach(x => this.FileList.Add(x));
            this.Index = this.FileList.IndexOf(currentPath);
            UpdateImage();

            //  resume watching after renaming.
            ResumeWatching();
        }

        public void DeleteImageFile()
        {
            //  stop watching to avoid multiple events triggered by deleting.
            StopWatching();

            FileFunction.DeleteFile(this.Current.FilePath);
            this.FileList.RemoveAt(_index);
            this.Index = _index switch
            {
                0 => 0,                                     //  if the first file is deleted, stay at index 0.
                var i when i == this.Length => _index - 1,  //  if the last file is deleted, move to the previous file.
                var i when i < _preview => _index - 1,      //  if the deleted file is before the current index, stay at the same index.
                _ => _index                                 //  if the deleted file is before the current index, stay at the same index; if it's after, move to the previous index.
            };
            UpdateImage();

            //  refresh file list after deleting.
            ResumeWatching();
        }

        public void MoveInImageFile(string sourcePath, string destinationName = null)
        {
            //  stop watching to avoid multiple events triggered by moving.
            StopWatching();

            var destinationPath = Path.Combine(
                this.Current.Parent,
                destinationName ?? Path.GetFileName(sourcePath));
            destinationPath = FileFunction.GetSafeNamePath(destinationPath);

            FileFunction.MoveFile(sourcePath, destinationPath);

            var collection = Directory.GetFiles(this.Current.Parent)
                .Where(x => IsValidImageFile(x))
                .OrderBy(x => x, new NaturalStringComparer())
                .ToList();
            this.FileList.Clear();
            collection.ForEach(x => this.FileList.Add(x));
            this.Index = this.FileList.IndexOf(destinationPath);
            UpdateImage();

            //  refresh file list after moving.
            ResumeWatching();
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

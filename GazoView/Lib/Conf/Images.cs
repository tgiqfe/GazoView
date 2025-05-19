using GazoView.Lib.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GazoView.Lib.Conf
{
    internal class Images : INotifyPropertyChanged
    {
        private static readonly string[] _validExtensions = new string[]
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".bmp",
            ".tiff",
            ".tif",
            ".gif",
            ".webp"
        };

        public ObservableCollection<string> FileList { get; private set; }

        public int Length { get { return FileList?.Count ?? 0; } }

        public ImageItem Current { get; private set; }

        public string Title
        {
            get
            {
                return FileList == null || FileList.Count == 0 ?
                    "[ 0 / 0 ] (No Image)" :
                    $"[ {Index + 1} / {FileList?.Count} ] {Current?.FileName} ({Current?.FilePath})";
            }
        }

        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                int length = Length;
                if (_index < 0)
                {
                    _index = length - 1;
                }
                else if (_index >= length)
                {
                    _index = 0;
                }
                if (length > 0)
                {
                    Current = new ImageItem(FileList[_index]);
                }

                OnPropertyChanged(nameof(Current));
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged();
            }
        }

        public Images(string[] targets)
        {
            LoadFiles(targets);
        }

        public void LoadFiles(string[] targets)
        {
            if (targets.Length == 1)
            {
                if (File.Exists(targets[0]))
                {
                    string parent = Path.GetDirectoryName(targets[0]);
                    var collection = Directory.GetFiles(parent).
                        Where(x => _validExtensions.Any(y => Path.GetExtension(x).ToLower() == y)).
                        OrderBy(x => x, new NaturalStringComparer());
                    this.FileList = new ObservableCollection<string>(collection);
                    this.Index = collection.ToList().IndexOf(targets[0]);
                }
                else if (Directory.Exists(targets[0]))
                {
                    string parent = targets[0];
                    var collection = Directory.GetFiles(parent).
                        Where(x => _validExtensions.Any(y => Path.GetExtension(x).ToLower() == y)).
                        OrderBy(x => x, new NaturalStringComparer());
                    this.FileList = new ObservableCollection<string>(collection);
                    this.Index = 0;
                }
            }
            else if (targets.Length > 1)
            {
                if (File.Exists(targets[0]))
                {
                    string parent = Path.GetDirectoryName(targets[0]);
                    var collection = targets.
                        Where(x =>
                        {
                            return File.Exists(x) &&
                                Path.GetDirectoryName(x) == parent &&
                                _validExtensions.Any(y => Path.GetExtension(x).ToLower() == y);
                        }).
                        OrderBy(x => x, new NaturalStringComparer());
                    this.FileList = new ObservableCollection<string>(collection);
                    this.Index = 0;
                }
            }
        }

        public void ReloadFiles(string movePath = null)
        {
            string moveToPath = movePath == null ?
                this.Current.FilePath :
                movePath;
            var collection = Directory.GetFiles(this.Current.Parent).
                Where(x => _validExtensions.Any(y => Path.GetExtension(x).ToLower() == y)).
                OrderBy(x => x, new NaturalStringComparer());
            this.FileList = new ObservableCollection<string>(collection);

            var index = FileList.IndexOf(moveToPath);
            if (index < 0) index = 0;

            this.Index = index;
        }

        public void TempChangeImage(ImageSource source)
        {
            string path = this.Current.FilePath;
            this.Current = new ImageItem(path, source);
            OnPropertyChanged(nameof(Current));
        }



        #region Inotify change

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}

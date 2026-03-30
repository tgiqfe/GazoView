using GazoView.Lib.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace GazoView.Lib
{
    public class Images : INotifyPropertyChanged
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
                        Where(x => _validExtensions.Any(y => Path.GetExtension(x).ToLower() == y)).
                        OrderBy(x => x, new NaturalStringComparer());
                    this.FileList = new ObservableCollection<string>(collection);
                    this.Index = this.FileList.IndexOf(targets[0]);
                    ViewImage();
                }
                else if (Directory.Exists(targets[0]))
                {
                    var collection = Directory.GetFiles(targets[0]).
                        Where(x => _validExtensions.Any(y => Path.GetExtension(x).ToLower() == y)).
                        OrderBy(x => x, new NaturalStringComparer());
                    this.FileList = new ObservableCollection<string>(collection);
                    this.Index = 0;
                    ViewImage();
                }
            }
        }

        public void ViewImage()
        {
            this.Current = new ImageItem(this.FileList[this.Index]);
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

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
using System.Windows;

namespace GazoView.Lib.Conf
{
    internal class Images : INotifyPropertyChanged
    {
        private static readonly string[] _validExtensions = new string[]
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".tif",
            ".tiff",
            ".bmp",
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
                if (_index < 0)
                {
                    _index = FileList?.Count - 1 ?? 0;
                }
                else if (_index >= FileList?.Count)
                {
                    _index = 0;
                }
                if (FileList?.Count > 0)
                {
                    Current = new ImageItem(FileList[_index]);
                }

                OnPropertyChanged(nameof(Current));
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged();
            }
        }

        public ScaleRate ScaleRate { get; set; }

        #region View image size

        private double _viewWidth = 0;
        private double _viewHeight = 0;

        public double ViewWidth
        {
            get { return _viewWidth; }
            set
            {
                _viewWidth = value;
                OnPropertyChanged(nameof(ImageScalePercent));
                OnPropertyChanged();
            }
        }
        public double ViewHeight
        {
            get { return _viewHeight; }
            set
            {
                _viewHeight = value;
                OnPropertyChanged();
            }
        }

        public double ImageScalePercent
        {
            get
            {
                if (Current == null) { return 0; }
                return _viewWidth / Current.Width;
            }
        }

        #endregion

        public Images(string[] targets)
        {
            LoadFiles(targets);
            ScaleRate = new();
        }

        /// <summary>
        /// 対象フォルダー配下の画像ファイルを読み込み
        /// </summary>
        /// <param name="targets"></param>
        public void LoadFiles(string[] targets)
        {
            if (targets.Length == 1)
            {
                if (File.Exists(targets[0]))
                {
                    //  ファイルを一つだけ指定
                    string parent = Path.GetDirectoryName(targets[0]);
                    var collection = Directory.GetFiles(parent).
                        Where(x => _validExtensions.Any(y => Path.GetExtension(x).ToLower() == y)).
                        OrderBy(x => x, new NaturalStringComparer());
                    FileList = new ObservableCollection<string>(collection);
                    Index = collection.ToList().IndexOf(targets[0]);
                }
                else if (Directory.Exists(targets[0]))
                {
                    //  フォルダーを一つだけ指定
                    string parent = targets[0];
                    var collection = Directory.GetFiles(parent).
                        Where(x => _validExtensions.Any(y => Path.GetExtension(x).ToLower() == y)).
                        OrderBy(x => x, new NaturalStringComparer());
                    FileList = new ObservableCollection<string>(collection);
                    Index = 0;
                }
            }
            else if (targets.Length > 1)
            {
                // 複数のファイルを指定
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
                    FileList = new ObservableCollection<string>(collection);
                    Index = 0;
                }
            }
        }

        /// <summary>
        /// 対象フォルダー配下の画像ファイルを再読み込み
        /// </summary>
        public void ReloadFiles()
        {
            string parent = this.Current.Parent;
            string selectedFile = this.Current.FilePath;
            var collection = Directory.GetFiles(parent).
                Where(x => _validExtensions.Any(y => Path.GetExtension(x).ToLower() == y)).
                OrderBy(x => x, new NaturalStringComparer());
            FileList = new ObservableCollection<string>(collection);

            var index = FileList.IndexOf(selectedFile);
            if (index < 0) index = 0;
            Index = index;
        }

        /// <summary>
        /// 現在選択中のファイルを削除
        /// </summary>
        public void Delete()
        {
            int index = this.Index;
            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(
                FileList[index],
                Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
            FileList.RemoveAt(index);
            if (FileList.Count == 0)
            {
                this.Current = null;
            }
            else
            {
                this.Index = index == FileList.Count ? index - 1 : index;
            }
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

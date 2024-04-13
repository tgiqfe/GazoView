using GazoView.Lib.Function;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
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

namespace GazoView.Lib.ImageInfo
{
    internal class ImageStore : INotifyPropertyChanged
    {
        /// <summary>
        /// 使用許可する拡張子
        /// </summary>
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

        private int _index = 0;

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
                    this.Current = Generator.Create(FileList[_index]);

                    OnPropertyChanged(nameof(Current));
                    OnPropertyChanged(nameof(Title));
                    OnPropertyChanged(nameof(ImageScalePercent));
                    OnPropertyChanged();
                }
            }
        }

        public string Title
        {
            get
            {
                return $"[ {this.Index + 1} / {FileList?.Count} ] {Current?.FileName} ({Current?.FilePath})";
            }
        }

        public ImageItem Current { get; private set; }

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

        /// <summary>
        /// 画像拡大率 (表示サイズ ÷ 画像サイズ)
        /// </summary>
        public double ImageScalePercent
        {
            get
            {
                if (Current == null) return 0;
                return _viewWidth / Current.Width;
            }
        }

        #endregion

        public ImageStore(string[] targets)
        {
            LoadFiles(targets);
            this.ScaleRate = new ScaleRate();
        }

        public void LoadFiles(string[] targets)
        {
            if (targets.Length == 1)
            {
                if (File.Exists(targets[0]))
                {
                    //  ファイルを1つだけ指定
                    string parent = Path.GetDirectoryName(targets[0]);
                    var collection = Directory.GetFiles(parent).
                        Where(x => _validExtensions.Any(y => Path.GetExtension(x).ToLower() == y)).
                        OrderBy(x => x, new NaturalStringComparer());
                    this.FileList = new ObservableCollection<string>(collection);
                    this.Index = collection.ToList().IndexOf(targets[0]);
                }
                else if (Directory.Exists(targets[0]))
                {
                    //  フォルダーを1つだけ指定
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
                //  複数のファイルを指定
                if (File.Exists(targets[0]))
                {
                    //  選択の1番最初がファイルの場合のみ複数選択
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

        public void Delete()
        {
            if (FileList.Count == 0) return;

            int index = this.Index;
            var ret = MessageBox.Show($"Delete: {FileList[index]}",
                "GazoView",
                MessageBoxButton.OKCancel,
                MessageBoxImage.None,
                MessageBoxResult.OK);
            if (ret == MessageBoxResult.OK)
            {
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(
                FileList[index],
                UIOption.OnlyErrorDialogs,
                RecycleOption.SendToRecycleBin);
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

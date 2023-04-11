using GazoView.Lib.Functions;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GazoView.Lib.Config
{
    internal class ImageStore : INotifyPropertyChanged
    {
        /// <summary>
        /// 使用可能な拡張子
        /// </summary>
        private static string[] _extensions = new string[]
        {
            ".jpg", ".jpeg", ".png", ".tif", ".tiff", ".svg", ".bmp", ".gif"
        };

        public string Parent { get; private set; }

        public List<string> FileList { get; private set; }

        private int _index = 0;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                if (_index <= 0)
                {
                    _index = FileList.Count;
                }
                else if (_index > FileList.Count)
                {
                    _index = 1;
                }
                this.Current = new BitmapImageItem(FileList[_index - 1]);

                OnPropertyChanged("ImageSource");
                OnPropertyChanged("Current");
                OnPropertyChanged();
            }
        }

        public IImageItem Current { get; private set; }

        public ImageSource ImageSource
        {
            get { return Current?.Source; }
        }

        #region Set image

        public ImageStore() { }

        public ImageStore(string[] paths)
        {
            SetFileList(paths);
        }

        public void UpdateFileList(string[] paths)
        {
            SetFileList(paths);
            OnPropertyChanged("FileList");
        }

        /// <summary>
        /// 画像ファイルのリストをセット
        /// </summary>
        /// <param name="paths"></param>
        private void SetFileList(string[] paths)
        {
            if (paths.Length == 1)
            {
                if (File.Exists(paths[0]))
                {
                    //  ファイルを1つのみ選択 (選択フォルダーと、同階層のファイルをリスト化)
                    this.Parent = Path.GetDirectoryName(paths[0]);
                    this.FileList = Directory.GetFiles(Parent).
                        Where(x => _extensions.Any(y => Path.GetExtension(x).Equals(y))).
                        OrderBy(x => x, new NaturalStringComparer()).
                        ToList();
                    this.Index = FileList.IndexOf(paths[0]) + 1;
                }
                else if (Directory.Exists(paths[0]))
                {
                    //  フォルダーを一つのみ選択 (選択フォルダー配下の全ファイルをリスト化)
                    this.Parent = paths[0];
                    this.FileList = Directory.GetFiles(Parent).
                        Where(x => _extensions.Any(y => Path.GetExtension(x).Equals(y))).
                        OrderBy(x => x, new NaturalStringComparer()).
                        ToList();
                    this.Index = 1;
                }
            }
            else if (paths.Length > 1)
            {
                if (File.Exists(paths[0]))
                {
                    //  ファイルを複数選択。少なくとも最初の1つがファイル (選択ファイルのみをリスト化)
                    this.Parent = Path.GetDirectoryName(paths[0]);
                    this.FileList = paths.
                        Where(x => File.Exists(x)).
                        Where(x => _extensions.Any(y => Path.GetExtension(x).Equals(y))).
                        OrderBy(x => x, new NaturalStringComparer()).
                        ToList();
                    this.Index = FileList.IndexOf(paths[0]) + 1;
                }
                else if (Directory.Exists(paths[0]))
                {
                    //  フォルダーを複数選択。少なくとも最初の1つがフォルダー (1つ目フォルダーの配下全ファイルをリスト化)
                    this.Parent = paths[0];
                    this.FileList = Directory.GetFiles(Parent).
                        Where(x => _extensions.Any(y => Path.GetExtension(x).Equals(y))).
                        OrderBy(x => x, new NaturalStringComparer()).
                        ToList();
                    this.Index = 1;
                }
            }
        }

        /// <summary>
        /// ファイルの削除
        /// </summary>
        /// <param name="force"></param>
        public void DeleteCurrentFile(bool force)
        {
            if (FileList?.Count > 0)
            {
                //  ★ここでファイルリストの自動サーチを一時停止

                int afterIndex = _index;
                if (afterIndex >= FileList.Count)
                {
                    afterIndex -= 1;
                }

                if (force)
                {
                    string target = Item.BindingParam.Images.Current.FilePath;
                    File.Delete(target);
                }
                else
                {
                    string target = Item.BindingParam.Images.Current.FilePath;
                    FileSystem.DeleteFile(target,
                        UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }

                this.FileList.RemoveAt(_index - 1);
                this.Index = afterIndex;
                OnPropertyChanged("FileList");

                //  ★ここでファイルリストの自動サーチを再開
            }
        }

        /// <summary>
        /// ファイルの移動
        /// </summary>
        public void MoveCurrentFile()
        {
            if (FileList?.Count > 0)
            {
                //  ★ここでファイルリストの自動サーチを一時停止

                int afterIndex = _index;
                if (afterIndex >= FileList.Count)
                {
                    afterIndex -= 1;
                }

                string source = Item.BindingParam.Images.Current.FilePath;
                var destination = FileAction.CreateSafePath(
                    Path.Combine(Parent, "temp", Item.BindingParam.Images.Current.FileName));
                FileSystem.MoveFile(source, destination);

                this.FileList.RemoveAt(_index - 1);
                this.Index = afterIndex;
                OnPropertyChanged("FileList");

                //  ★ここでファイルリストの自動サーチを再開
            }
        }

        #endregion
        #region Inotify change

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}

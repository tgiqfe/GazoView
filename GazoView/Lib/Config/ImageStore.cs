using GazoView.Lib.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        private List<string> FileList = null;

        private int _index = 0;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                if (_index < 0)
                {
                    _index = FileList.Count - 1;
                }
                else if (_index >= FileList.Count)
                {
                    _index = 0;
                }
                this.Current = new BitmapImageItem(FileList[Index]);
                OnPropertyChanged("ImageSource");
            }
        }

        public IImageItem Current { get; private set; }

        public ImageSource ImageSource
        {
            get { return Current.Source; }
        }

        #region Constructor

        public ImageStore() { }

        public ImageStore(string[] paths)
        {
            string path = paths[0];

            if (File.Exists(path))
            {
                this.Parent = Path.GetDirectoryName(path);
                this.FileList = Directory.GetFiles(Parent).
                    Where(x => _extensions.Any(y => Path.GetExtension(x).Equals(y))).
                    OrderBy(x => x, new NaturalStringComparer()).
                    ToList();
                this.Index = FileList.IndexOf(path);
            }
            else if (Directory.Exists(path))
            {
                this.Parent = path;
                this.FileList = Directory.GetFiles(Parent).
                    Where(x => _extensions.Any(y => Path.GetExtension(x).Equals(y))).
                    OrderBy(x => x, new NaturalStringComparer()).
                    ToList();
                this.Index = 0;
            }
        }

        #endregion
        #region Inotify change

        /// <summary>
        /// 変更通知用
        /// 参考)
        /// https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/data/how-to-implement-property-change-notification?view=netframeworkdesktop-4.8
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 変更通知用メソッド
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}

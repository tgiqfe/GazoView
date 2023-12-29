using GazoView.Lib.ImageInfo;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using GazoView.Lib.Functions;

namespace GazoView.Lib
{
    internal class ImageStore : INotifyPropertyChanged
    {
        public string Parent { get; private set; }

        public List<string> FileList { get; private set; }

        public int _index = 0;

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
                    this.Current = ImageItemGenerator.Create(FileList[_index]);

                    OnPropertyChanged("ImageSource");
                    OnPropertyChanged("Current");
                    OnPropertyChanged("TitleMessage");
                    OnPropertyChanged();
                }
            }
        }

        public BaseImageItem Current { get; private set; }

        public ImageSource ImageSource { get { return Current?.Source; } }

        public string TitleMessage
        {
            get
            {
                return $"[ {Index + 1} / {FileList?.Count} ] {Current?.FileName} ({Current?.FilePath})";
            }
        }

        public bool IsAllFiles { get; set; }


        public ImageStore(string[] targets)
        {
            SetFileList(targets);
        }

        public void SetFileList(string[] targets)
        {
            if (targets.Length == 1)
            {
                //  ファイルを1つだけ指定
                if (File.Exists(targets[0]))
                {
                    this.Parent = Path.GetDirectoryName(targets[0]);
                    this.FileList = Directory.GetFiles(Parent).
                        Where(x => Item.ValidExtensions.Any(y => Path.GetExtension(x).ToLower() == y)).
                        OrderBy(x => x, new NaturalStringComparer()).
                        ToList();

                    this.FileList = new List<string>(Directory.GetFiles(Parent));
                    this.Index = FileList.IndexOf(targets[0]);
                    this.IsAllFiles = true;
                }
                else if (Directory.Exists(targets[0]))
                {
                    //  フォルダーを1つだけ指定
                    this.Parent = targets[0];
                    this.FileList = Directory.GetFiles(Parent).
                        Where(x => Item.ValidExtensions.Any(y => Path.GetExtension(x).ToLower() == y)).
                        OrderBy(x => x, new NaturalStringComparer()).
                        ToList();
                    this.Index = 0;
                    this.IsAllFiles = true;
                }
            }
            else if (targets.Length >= 2)
            {

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

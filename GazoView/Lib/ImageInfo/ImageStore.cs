using GazoView.Lib.ImageInfo;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using GazoView.Lib.Functions;
using GazoView.Conf;

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

                    OnPropertyChanged("Current");
                    OnPropertyChanged("TitleMessage");
                    OnPropertyChanged();
                }
            }
        }

        #region ViewSize

        private double _viewwidth = 0;
        private double _viewheight = 0;

        public double ViewWidth
        {
            get { return _viewwidth; }
            set
            {
                _viewwidth = value;
                OnPropertyChanged("ImageScalePercent");
                OnPropertyChanged();
            }
        }

        public double ViewHeight
        {
            get { return _viewheight; }
            set
            {
                _viewheight = value;
                OnPropertyChanged();
            }
        }

        public double ImageScalePercent
        {
            get { return _viewwidth / this.Current.Width; }
        }

        #endregion

        public BaseImageItem Current { get; private set; }

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

        #region Scaling parameter

        /// <summary>
        /// 変更後の拡大率
        /// </summary>
        public double Scale { get { return _ticks[_tickindex]; } }

        /// <summary>
        /// ひとつ前の拡大率
        /// </summary>
        public double PreviewScale { get { return _ticks[_previewtickindex]; } }

        /// <summary>
        /// 最大拡大率かどうか
        /// </summary>
        public bool IsMaxScale { get { return _tickindex == _ticks.Length - 1; } }

        /// <summary>
        /// 最小拡大率かどうか
        /// </summary>
        public bool IsMinScale { get { return _tickindex == 0; } }

        /// <summary>
        /// 拡大率の目盛り
        /// </summary>
        protected static readonly double[] _ticks = new double[]
        {
            0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.8, 2, 2.4, 2.8, 3.2, 3.6, 4, 4.8, 5.6, 6.4, 7.2, 8, 9, 10
        };

        /// <summary>
        /// 拡大率の目盛りのインデックス
        /// </summary>
        private int _tickindex = 8;

        /// <summary>
        /// ひとつ前の拡大率の目盛りのインデックス
        /// </summary>
        private int _previewtickindex = 8;

        /// <summary>
        /// 拡大率の目盛りのインデックス(外部からの操作用)
        /// </summary>
        public int TickIndex
        {
            get { return _tickindex; }
            set
            {
                _previewtickindex = _tickindex;
                _tickindex = value;
                OnPropertyChanged("Scale");
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

using System.Windows;
using GazoView.Lib.Functions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.IO;

namespace GazoView.Lib
{
    public class Trimming : INotifyPropertyChanged
    {
        /// <summary>
        /// TrimmingMode on / off.
        /// </summary>
        private bool _isTrimmingMode = false;
        public bool IsTrimmingMode
        {
            get { return _isTrimmingMode; }
            set
            {
                _isTrimmingMode = value;
                OnPropertyChanged();
            }
        }

        #region Trimming area values (Top, Bottom, Left, Right)

        public int Top
        {
            get => _setting.TrimmingTop;
            set
            {
                if (_setting.TrimmingTop != value)
                {
                    _setting.TrimmingTop = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AssistTop));
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        public int Bottom
        {
            get => _setting.TrimmingBottom;
            set
            {
                if (_setting.TrimmingBottom != value)
                {
                    _setting.TrimmingBottom = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AssistBottom));
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        public int Left
        {
            get => _setting.TrimmingLeft;
            set
            {
                if (_setting.TrimmingLeft != value)
                {
                    _setting.TrimmingLeft = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AssistLeft));
                    OnPropertyChanged(nameof(Width));
                }
            }
        }

        public int Right
        {
            get => _setting.TrimmingRight;
            set
            {
                if (_setting.TrimmingRight != value)
                {
                    _setting.TrimmingRight = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AssistRight));
                    OnPropertyChanged(nameof(Width));
                }
            }
        }

        #endregion

        public int Width { get => this.Right - this.Left; }
        public int Height { get => this.Bottom - this.Top; }
        public string TrimmedResolution { get => $"{this.Width} x {this.Height}"; }

        //  Assist values for assist line display (4px outside of trimming area).
        public double AssistTop { get => this.Top - 4; }
        public double AssistBottom { get => this.Bottom + 4; }
        public double AssistLeft { get => this.Left - 4; }
        public double AssistRight { get => this.Right + 4; }

        public List<TrimHistory> TrimHistories = null;
        public int TrimHistoryIndex { get; set; }

        // Setting reference for save trimming setting.
        private Setting _setting = null;

        public Trimming(Setting setting)
        {
            _setting = setting;
            this.TrimHistories = new();
            /*
            this.TrimHistories.Add(new()
            {
                Top = _setting.TrimmingTop,
                Bottom = _setting.TrimmingBottom,
                Left = _setting.TrimmingLeft,
                Right = _setting.TrimmingRight
            });
            */
        }

        public void SwitchMode(bool? toEnable = null)
        {
            IsTrimmingMode = toEnable ?? !IsTrimmingMode;
        }

        /// <summary>
        /// Trimming start process.
        /// It crops the current image based on trimming area values and saves it as a new file.
        /// </summary>
        public void StartTrimming()
        {
            string outputPath = FileFunction.GetSafeNamePath(Item.BindingParam.Images.Current.FilePath);

            var scale = Item.MainWindow.ImagePanel.DesiredSize.Width / Item.BindingParam.Images.Current.Width;
            var (left, top, width, height) = (
                (int)(this.Left / scale),
                (int)(this.Top / scale),
                (int)(this.Width / scale),
                (int)(this.Height / scale)
            );

            if (Item.BindingParam.Images.Current.Source is BitmapSource imgSrc)
            {
                var bitmap = new CroppedBitmap(imgSrc, new Int32Rect(left, top, width, height));
                using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    BitmapEncoder encoder = Item.BindingParam.Images.Current.FileExtension.ToLower() switch
                    {
                        ".jpg" or ".jpeg" => new JpegBitmapEncoder(),
                        ".png" => new PngBitmapEncoder(),
                        ".tif" or ".tiff" => new TiffBitmapEncoder(),
                        ".bmp" => new BmpBitmapEncoder(),
                        _ => null
                    };
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    encoder.Save(fs);
                }
                bitmap.Freeze();
            }
        }

        #region Trim history management

        public void AddTrimHistory()
        {
            if (this.TrimHistories.Count == 0)
            {
                this.TrimHistories.Add(new TrimHistory()
                {
                    Top = this.Top,
                    Bottom = this.Bottom,
                    Left = this.Left,
                    Right = this.Right
                });
                this.TrimHistoryIndex = 0;
                return;
            }
            if (this.TrimHistoryIndex >= 0 &&
               this.TrimHistories[this.TrimHistoryIndex].Top == this.Top &&
               this.TrimHistories[this.TrimHistoryIndex].Bottom == this.Bottom &&
               this.TrimHistories[this.TrimHistoryIndex].Left == this.Left &&
               this.TrimHistories[this.TrimHistoryIndex].Right == this.Right)
            {
                // No change in trimming area, do not add history.
                return;
            }
            if (this.TrimHistories.Count > this.TrimHistoryIndex + 1)
            {
                this.TrimHistories.RemoveRange(
                    this.TrimHistoryIndex + 1,
                    this.TrimHistories.Count - (this.TrimHistoryIndex + 1));
            }
            this.TrimHistories.Add(new TrimHistory()
            {
                Top = this.Top,
                Bottom = this.Bottom,
                Left = this.Left,
                Right = this.Right
            });
            this.TrimHistoryIndex++;
        }

        public void UndoTrimHistory()
        {
            if (this.TrimHistoryIndex > 0)
            {
                this.TrimHistoryIndex--;
                var history = this.TrimHistories[this.TrimHistoryIndex];
                this.Top = history.Top;
                this.Bottom = history.Bottom;
                this.Left = history.Left;
                this.Right = history.Right;
            }
        }

        public void RedoTrimHistory()
        {
            if (this.TrimHistoryIndex < this.TrimHistories.Count - 1)
            {
                this.TrimHistoryIndex++;
                var history = this.TrimHistories[this.TrimHistoryIndex];
                this.Top = history.Top;
                this.Bottom = history.Bottom;
                this.Left = history.Left;
                this.Right = history.Right;
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

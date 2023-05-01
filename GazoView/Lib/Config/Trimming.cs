using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GazoView.Lib.Config
{
    public class Trimming : INotifyPropertyChanged
    {
        private const int LINE_THICKNES_HALF = 4;

        /// <summary>
        /// コントロール最上部からTopエリアの高さ
        /// </summary>
        private int _top = 0;

        /// <summary>
        /// コントロール最上部からBottomエリアまでの位置
        /// </summary>
        private int _bottom = 0;

        /// <summary>
        /// コントロール最左部からLeftエリアの幅
        /// </summary>
        private int _left = 0;

        /// <summary>
        /// コントロール最左部からRightエリアまでの位置
        /// </summary>
        private int _right = 0;

        public int Top
        {
            get { return _top; }
            set
            {
                _top = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BorderTop));
                OnPropertyChanged(nameof(AreaTop));
                OnPropertyChanged(nameof(Height));
            }
        }

        public int Bottom
        {
            get { return _bottom; }
            set
            {
                _bottom = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BorderBottom));
                OnPropertyChanged(nameof(AreaBottom));
                OnPropertyChanged(nameof(Height));
            }
        }

        public int Left
        {
            get { return _left; }
            set
            {
                _left = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BorderLeft));
                OnPropertyChanged(nameof(AreaLeft));
                OnPropertyChanged(nameof(Width));
            }
        }

        public int Right
        {
            get { return _right; }
            set
            {
                _right = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BorderRight));
                OnPropertyChanged(nameof(AreaRight));
                OnPropertyChanged(nameof(Width));
            }
        }

        [JsonIgnore]
        public int AreaTop
        {
            get { return _top; }
        }

        [JsonIgnore]
        public int AreaBottom
        {
            get { return (int)Item.Layer.ActualHeight - _bottom; }
        }

        [JsonIgnore]
        public int AreaLeft
        {
            get { return _left; }
        }

        [JsonIgnore]
        public int AreaRight
        {
            get { return (int)Item.Layer.ActualWidth - _right; }
        }

        [JsonIgnore]
        public int BorderTop
        {
            get { return _top - LINE_THICKNES_HALF; }
        }

        [JsonIgnore]
        public int BorderBottom
        {
            get { return _bottom + LINE_THICKNES_HALF; }
        }

        [JsonIgnore]
        public int BorderLeft
        {
            get { return _left - LINE_THICKNES_HALF; }
        }

        [JsonIgnore]
        public int BorderRight
        {
            get { return _right + LINE_THICKNES_HALF; }
        }

        [JsonIgnore]
        public int Width { get { return _right - _left; } }

        [JsonIgnore]
        public int Height { get { return _bottom - _top; } }

        public void GrayAreaReload()
        {
            OnPropertyChanged(nameof(AreaTop));
            OnPropertyChanged(nameof(AreaBottom));
            OnPropertyChanged(nameof(AreaLeft));
            OnPropertyChanged(nameof(AreaRight));
        }

        public override string ToString()
        {
            return $"L:{Left}, T:{Top}, R:{Right}, B:{Bottom} ( {Width} x {Height} )";
        }

        public override bool Equals(object obj)
        {
            if (obj is Trimming trim)
            {
                return this.Top == trim.Top &&
                    this.Bottom == trim.Bottom &&
                    this.Left == trim.Left &&
                    this.Right == trim.Right;
            }
            return false;
        }

        public Trimming() { }

        public Trimming(int top, int bottom, int left, int right)
        {
            _top = top;
            _bottom = bottom;
            _left = left;
            _right = right;
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

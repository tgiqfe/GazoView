using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

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

        private double _scale = 1;

        private int _top = -1;
        public int Top
        {
            get => _top;
            set
            {
                if (_top != value)
                {
                    _top = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ViewTop));
                }
            }
        }

        private int _bottom = -1;
        public int Bottom
        {
            get => _bottom;
            set
            {
                if (_bottom != value)
                {
                    _bottom = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ViewBottom));
                }
            }
        }
        
        private int _left = -1;
        public int Left
        {
            get => _left;
            set
            {
                if (_left != value)
                {
                    _left = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ViewLeft));
                }
            }
        }

        private int _right = -1;
        public int Right
        {
            get => _right;
            set
            {
                if (_right != value)
                {
                    _right = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ViewRight));
                }
            }
        }

        public double ViewTop { get { return this.Top * this._scale; } }
        public double ViewBottom { get { return this.Bottom * this._scale; } } 
        public double ViewLeft { get { return this.Left * this._scale; } }
        public double ViewRight { get { return this.Right * this._scale; } }

        #region Constructor

        public Trimming(int top, int bottom, int left, int right)
        {
            this.Top = top;
            this.Bottom = bottom;
            this.Left = left;
            this.Right = right;
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

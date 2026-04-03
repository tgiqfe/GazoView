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

        //private int _top = -1;
        public int Top
        {
            //get => _top;
            get => _setting.TrimmingTop;
            set
            {
                if (_setting.TrimmingTop != value)
                {
                    _setting.TrimmingTop = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ViewTop));
                }
            }
        }

        //private int _bottom = -1;
        public int Bottom
        {
            //get => _bottom;
            get => _setting.TrimmingBottom;
            set
            {
                if (_setting.TrimmingBottom != value)
                {
                    _setting.TrimmingBottom = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ViewBottom));
                }
            }
        }
        
        //private int _left = -1;
        public int Left
        {
            //get => _left;
            get => _setting.TrimmingLeft;
            set
            {
                if (_setting.TrimmingLeft != value)
                {
                    _setting.TrimmingLeft = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ViewLeft));
                }
            }
        }

        //private int _right = -1;
        public int Right
        {
            //get => _right;
            get => _setting.TrimmingRight;
            set
            {
                if (_setting.TrimmingRight != value)
                {
                    _setting.TrimmingRight = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ViewRight));
                }
            }
        }

        public double ViewTop { get { return this.Top * this._scale; } }
        public double ViewBottom { get { return this.Bottom * this._scale; } } 
        public double ViewLeft { get { return this.Left * this._scale; } }
        public double ViewRight { get { return this.Right * this._scale; } }


        // Setting reference for save trimming setting.
        // If referencing the settings doesn't work, consider other methods.
        private Setting _setting = null;

        public Trimming(Setting setting)
        {
            _setting = setting;
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

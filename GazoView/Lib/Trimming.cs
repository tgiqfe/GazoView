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

        public int Top
        {
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

        public int Bottom
        {
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
        
        public int Left
        {
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

        public int Right
        {
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

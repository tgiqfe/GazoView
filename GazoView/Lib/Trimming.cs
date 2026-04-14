using System.ComponentModel;
using System.Runtime.CompilerServices;

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
                    OnPropertyChanged(nameof(AssistTop));
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
                    OnPropertyChanged(nameof(AssistBottom));
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
                    OnPropertyChanged(nameof(AssistLeft));
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
                    OnPropertyChanged(nameof(AssistRight));
                }
            }
        }

        public double ViewTop { get { return this.Top * this._scale; } }
        public double ViewBottom { get { return this.Bottom * this._scale; } }
        public double ViewLeft { get { return this.Left * this._scale; } }
        public double ViewRight { get { return this.Right * this._scale; } }


        //  Assist values for assist line display (4px outside of trimming area).
        public double AssistTop { get => this.Top - 4; }
        public double AssistBottom { get => this.Bottom + 4; }
        public double AssistLeft { get => this.Left - 4; }
        public double AssistRight { get => this.Right + 4; }

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

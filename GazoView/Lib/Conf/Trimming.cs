using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GazoView.Lib.Conf
{
    internal class Trimming : INotifyPropertyChanged
    {
        private double _scale = 1;

        public double Scale
        {
            get { return _scale; }
            set
            {
                if (_scale != value)
                {
                    _scale = value;
                    OnPropertyChanged(nameof(ViewTop));
                    OnPropertyChanged(nameof(ViewBottom));
                    OnPropertyChanged(nameof(ViewLeft));
                    OnPropertyChanged(nameof(ViewRight));
                }
            }
        }

        #region View size

        public double ViewTop { get { return this.Top * this._scale; } }
        public double ViewBottom { get { return this.Bottom * this._scale; } }
        public double ViewLeft { get { return this.Left * this._scale; } }
        public double ViewRight { get { return this.Right * this._scale; } }

        #endregion
        #region Raw size

        private int _top = -1;
        public int Top
        {
            get { return _top; }
            set
            {
                _top = value;
                OnPropertyChanged(nameof(ViewTop));
                OnPropertyChanged();
            }
        }

        private int _bottom = -1;
        public int Bottom
        {
            get { return _bottom; }
            set
            {
                _bottom = value;
                OnPropertyChanged(nameof(ViewBottom));
                OnPropertyChanged();
            }
        }

        private int _left = -1;
        public int Left
        {
            get { return _left; }
            set
            {
                _left = value;
                OnPropertyChanged(nameof(ViewLeft));
                OnPropertyChanged();
            }
        }

        private int _right = -1;
        public int Right
        {
            get { return _right; }
            set
            {
                _right = value;
                OnPropertyChanged(nameof(ViewRight));
                OnPropertyChanged();
            }
        }

        #endregion
        #region border location size

        const int BORDER_HALF_WIDTH = 4;

        public double BorderTop { get { return this.ViewTop + BORDER_HALF_WIDTH; } }
        public double BorderBottom { get { return this.ViewBottom - BORDER_HALF_WIDTH; } }
        public double BorderLeft { get { return this.ViewLeft + BORDER_HALF_WIDTH; } }
        public double BorderRight { get { return this.ViewRight - BORDER_HALF_WIDTH; } }

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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Conf
{
    internal class Trimming : INotifyPropertyChanged
    {
        private double _top = 100;
        private double _bottom = 300;
        private double _left = 100;
        private double _right = 300;

        public double Top
        {
            get { return _top; }
            set
            {
                _top = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AreaTop));
            }
        }
        public double Bottom
        {
            get { return _bottom; }
            set
            {
                _bottom = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AreaBottom));
            }
        }
        public double Left
        {
            get { return _left; }
            set
            {
                _left = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AreaLeft));
            }
        }
        public double Right
        {
            get { return _right; }
            set
            {
                _right = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AreaRight));
            }
        }

        public double AreaTop { get { return _top; } }
        public double AreaBottom { get { return Item.Mainbase.MainImage.ActualHeight - _bottom; } }
        public double AreaLeft { get { return _left; } }
        public double AreaRight { get { return Item.Mainbase.MainImage.ActualWidth - _right; } }

        public void GrayAreaReload()
        {
            OnPropertyChanged(nameof(AreaTop));
            OnPropertyChanged(nameof(AreaBottom));
            OnPropertyChanged(nameof(AreaLeft));
            OnPropertyChanged(nameof(AreaRight));
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GazoView.Conf
{
    internal class Trimming : INotifyPropertyChanged
    {
        private double _scale = 1;

        public double Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                OnPropertyChanged(nameof(ViewTop));
                OnPropertyChanged(nameof(ViewBottom));
                OnPropertyChanged(nameof(ViewLeft));
                OnPropertyChanged(nameof(ViewRight));
            }
        }

        const int BORDER_HALF_WIDTH = 4;

        private double _top = 100;
        private double _bottom = 300;
        private double _left = 100;
        private double _right = 300;

        public double Top
        {
            get { return _top; }
            set { _top = value; OnPropertyChanged(nameof(ViewTop)); }
        }
        public double Bottom
        {
            get { return _bottom; }
            set { _bottom = value; OnPropertyChanged(nameof(ViewBottom)); }
        }
        public double Left
        {
            get { return _left; }
            set { _left = value; OnPropertyChanged(nameof(ViewLeft)); }
        }
        public double Right
        {
            get { return _right; }
            set { _right = value; OnPropertyChanged(nameof(ViewRight)); }
        }

        public double ViewTop { get { return this.Top * this.Scale; } }
        public double ViewBottom { get { return this.Bottom * this.Scale; } }
        public double ViewLeft { get { return this.Left * this.Scale; } }
        public double ViewRight { get { return this.Right * this.Scale; } }

        public double BorderTop { get { return this.ViewTop + BORDER_HALF_WIDTH; } }
        public double BorderBottom { get { return this.ViewBottom - BORDER_HALF_WIDTH; } }
        public double BorderLeft { get { return this.ViewLeft + BORDER_HALF_WIDTH; } }
        public double BorderRight { get { return this.ViewRight - BORDER_HALF_WIDTH; } }



        #region Inotify change

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}

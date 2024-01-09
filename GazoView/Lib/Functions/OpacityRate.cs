using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Functions
{
    internal class OpacityRate : INotifyPropertyChanged
    {
        private static readonly double[] _ticks = new double[]
        {
            0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1
        };

        /// <summary>
        /// 初期値: _ticksの一番最後 (値: 1)
        /// </summary>
        private int _index = _ticks.Length - 1;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                if (_index < 0) _index = 0;
                if (_index >= _ticks.Length) _index = _ticks.Length - 1;

                OnPropertyChanged("Value");
            }
        }

        public double Value { get { return _ticks[_index]; } }

        #region Inotify change

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}

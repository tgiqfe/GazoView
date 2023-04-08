using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Config
{
    internal class OpacityRate : INotifyPropertyChanged
    {
        private double[] _ticks = new double[]
        {
            0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9
        };

        /// <summary>
        /// 初期値: 4 (_ticksのインデックス 4 = 0.5)
        /// </summary>
        private int _index = 4;

        /// <summary>
        /// 透明度の度合
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                if (_index < 0) { _index = 0; }
                if (_index >= _ticks.Length) { _index = _ticks.Length - 1; }
                OnPropertyChanged("Value");
                OnPropertyChanged("Percent");
            }
        }

        /// <summary>
        /// 現在のインデックス値から不透明度を取得
        /// </summary>
        public double Value { get { return _ticks[this.Index]; } }

        /// <summary>
        /// 現在の不透明度のパーセントを取得
        /// </summary>
        public string Percent
        {
            get
            {
                return this.Enabled ?
                    ((int)(_ticks[this.Index] * 100)).ToString() :
                    "-";
            }
        }

        /// <summary>
        /// 透明モードの有効/無効 private
        /// </summary>
        private bool _enabled = false;

        /// <summary>
        /// 透明モードの有効/無効
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                OnPropertyChanged("Percent");
            }
        }

        #region Notify change

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}

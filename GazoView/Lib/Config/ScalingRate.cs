using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Config
{
    internal class ScalingRate : INotifyPropertyChanged
    {
        private static readonly double[] _ticks = new double[]
        {
            0.2, 0.25, 0.3, 0.35, 0.4, 0.45, 0.5, 0.6, 0.7, 0.8, 0.9, 1, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.8, 2, 2.2, 2.4, 2.6, 2.8, 3, 3.2, 3.6, 4, 4.8, 5.6, 6.4, 7.2, 8
        };

        /// <summary>
        /// 初期値: 11 (_ticksのインデックス 11 = 100)
        /// </summary>
        const int DEF_INDEX = 11;

        /// <summary>
        /// インデックス番号の本体
        /// </summary>
        private int _index = DEF_INDEX;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                if (_index < 0) { _index = 0; }
                if (_index >= _ticks.Length) { _index = _ticks.Length - 1; }
                OnPropertyChanged("Percent");
            }
        }

        /// <summary>
        /// 現在のインデックス値から拡大率を取得
        /// </summary>
        public double Value
        {
            get { return _ticks[this.Index]; }
            set
            {
                this.Index = _ticks.Contains(value) ?
                    _ticks.ToList().IndexOf(value) :
                    DEF_INDEX;
            }
        }

        /// <summary>
        /// 変更前の拡大率を取得
        /// </summary>
        public double PrevValue { get; set; } = _ticks[DEF_INDEX];

        /// <summary>
        /// 現在の拡大率のパーセントを取得
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
        /// 拡縮モードの有効/無効 private
        /// </summary>
        private bool _enabled = false;

        /// <summary>
        /// 拡縮モードの有効/無効
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

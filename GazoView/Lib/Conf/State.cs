using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GazoView.Lib.Conf
{
    internal class State : INotifyPropertyChanged
    {
        /// <summary>
        /// Infoパネルの表示状態
        /// </summary>
        public bool InfoPanel { get; set; }

        /// <summary>
        /// 拡縮モード
        /// </summary>
        private bool _scalingMode = false;
        public bool ScalingMode
        {
            get { return _scalingMode; }
            set
            {
                _scalingMode = value;
                OnPropertyChanged();
            }
        }

        private bool _trimmingMode = false;

        public bool TrimmingMode
        {
            get { return _trimmingMode; }
            set
            {
                _trimmingMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Bitmapの拡大縮小時の最近傍法を使用するかどうか
        /// </summary>
        private bool _nearestNeighbor = false;
        public bool NearestNeighbor
        {
            get { return _nearestNeighbor; }
            set
            {
                _nearestNeighbor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// TrimmingパネルのTextBoxにフォーカスが当たっているかどうか
        /// </summary>
        public bool IsTrimmingSizeChanging { get; set; }

        #region Inotify change

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Conf
{
    internal class State : INotifyPropertyChanged
    {
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

        /// <summary>
        /// トリミングモード
        /// </summary>
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
        /// Bitmapの拡大縮小時に最近傍法を使用するか
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

        /*
        /// <summary>
        /// Infoパネルの表示/非表示
        /// </summary>
        private bool _showInfoPanel = false;

        public bool ShowInfoPanel
        {
            get { return _showInfoPanel; }
            set
            {
                _showInfoPanel = value;
                OnPropertyChanged();
            }
        }
        */

        /// <summary>
        /// Infoパネルの表示状態
        /// 0 ⇒ 非表示
        /// 1 ⇒ 左側に表示(重ねて表示)
        /// 2 ⇒ 左側に表示
        /// </summary>
        private int _infoPanelIndex = 0;

        public int InfoPanelIndex
        {
            get { return _infoPanelIndex; }
            set
            {
                _infoPanelIndex = value;
                OnPropertyChanged();
            }
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

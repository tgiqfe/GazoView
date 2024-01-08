using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Actions
{
    internal class State : INotifyPropertyChanged
    {
        /// <summary>
        /// 拡縮モード(private)
        /// </summary>
        private bool _scalingMode = false;

        public bool ScalingMode
        {
            get { return _scalingMode; }
            set { _scalingMode = value; OnPropertyChanged(); }
        }

        //  トリミングモード


        //  透明化モード


        /// <summary>
        /// Bitmapスケーリングモード
        /// true ⇒ NearestNeighbor
        /// false ⇒ Fant
        /// </summary>
        private bool _neirestNeighbor = false;

        public bool NearestNeighbor
        {
            get { return _neirestNeighbor; }
            set { _neirestNeighbor = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// Infoパネルの表示/非表示
        /// </summary>
        private bool _showInfoPanel = false;

        public bool ShowInfoPanel
        {
            get { return _showInfoPanel; }
            set { _showInfoPanel = value; OnPropertyChanged(); }
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GazoView.Lib.Conf
{
    internal class State : INotifyPropertyChanged
    {
        #region InfoPanel state

        private InfoPanelStatus _infopanelStatus = InfoPanelStatus.None;

        public enum InfoPanelStatus
        {
            None = 0,
            OverlapLeft = 1,
            ViewLeft = 2,
            Last = 3,
        }

        public InfoPanelStatus InfoPanel
        {
            get { return _infopanelStatus; }
            set
            {
                _infopanelStatus = value;
                if (_infopanelStatus < 0)
                {
                    _infopanelStatus = InfoPanelStatus.Last - 1;
                }
                else if (_infopanelStatus >= InfoPanelStatus.Last)
                {
                    _infopanelStatus = InfoPanelStatus.None;
                }
            }
        }

        public bool InfoPanel2 { get; set; }

        #endregion
        #region TrimmingMode state

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

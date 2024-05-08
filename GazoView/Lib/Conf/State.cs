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

        /// <summary>
        /// Infoパネルの表示状態
        /// </summary>
        public bool InfoPanel { get; set; }

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

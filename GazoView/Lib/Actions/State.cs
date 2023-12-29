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




        #region Inotify change

        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}

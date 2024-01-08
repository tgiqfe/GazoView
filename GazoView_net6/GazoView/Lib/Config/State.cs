using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Config
{
    public class State : INotifyPropertyChanged
    {
        /// <summary>
        /// トリミングモード private
        /// </summary>
        private bool _trimmingMode = false;

        /// <summary>
        /// トリミングモード
        /// </summary>
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
        /// 拡縮モード private
        /// </summary>
        private bool _scalingMode = false;


        /// <summary>
        /// 拡縮モード
        /// </summary>
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
        /// 透明化モード private
        /// </summary>
        private bool _transparentMode = false;

        /// <summary>
        /// 透明化モード
        /// </summary>
        public bool TransparentMode
        {
            get { return _transparentMode; }
            set
            {
                _transparentMode = value;
                OnPropertyChanged();
            }
        }

        #region Inotify change

        /// <summary>
        /// 変更通知用
        /// 参考)
        /// https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/data/how-to-implement-property-change-notification?view=netframeworkdesktop-4.8
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 変更通知用メソッド
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GazoView.Lib.Conf
{
    internal class State : INotifyPropertyChanged
    {
        public string Version { get { return Item.Version; } }


        private bool _scalingMode = false;
        public bool ScalingMode
        {
            get { return _scalingMode; }
            set
            {
                if (_scalingMode != value)
                {
                    _scalingMode = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isGifAnimationView = false;
        public bool IsGifAnimationView
        {
            get { return _isGifAnimationView; }
            set
            {
                _isGifAnimationView = value;
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

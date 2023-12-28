using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GazoView.Lib
{
    internal class ImageStore : INotifyPropertyChanged
    {
        public string Paremt { get; private set; }

        public List<string> FileList { get; private set; }

        public int _index = 0;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                if (_index < 0)
                {
                    _index = FileList?.Count - 1 ?? 0;
                }
                else if (_index >= FileList?.Count)
                {
                    _index = 0;
                }
                if (FileList?.Count > 0)
                {




                }
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

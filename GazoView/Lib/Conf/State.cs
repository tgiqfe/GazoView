using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GazoView.Lib.Conf
{
    internal class State
    {
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
    }
}

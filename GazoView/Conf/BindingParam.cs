using GazoView.Lib;
using GazoView.Lib.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Conf
{
    class BindingParam
    {
        public Setting Setting { get; set; }

        public ImageStore Images { get; set; }

        public State State { get; set; }


        public BindingParam(string[] targets)
        {
            Setting = Setting.Load();
            Images = new ImageStore(targets);
            State = new State();
        }
    }
}

using GazoView.Conf;
using GazoView.Lib.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib
{
    class BindingParam
    {
        public Setting Setting { get; set; }

        public ImageStore Images { get; set; }

        public State State { get; set; }


        public BindingParam(string[] targets)
        {
            this.Setting = Setting.Load();
            this.Images = new ImageStore(targets);
            this.State = new State();
        }
    }
}

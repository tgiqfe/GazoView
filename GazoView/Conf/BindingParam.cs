using GazoView.Lib.ImageInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Conf
{
    internal class BindingParam
    {
        public Setting Setting { get; set; }

        public ImageStore Images { get; set; }

        public State State { get; set; }

        public Trimming Trimming { get; set; }

        public BindingParam(string[] targets)
        {
            this.Setting = Setting.Load();
            this.Images = new(targets);
            this.State = new();
            this.Trimming = new();
        }
    }
}

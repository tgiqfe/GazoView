using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GazoView.Lib
{
    public class BindingParam
    {
        public Setting Setting { get; set; }

        public Images Images { get; set; }

        public Trimming Trimming { get; set; }

        public BindingParam(string[] imageFileTargets)
        {
            var setting = Setting.Load();

            this.Setting = setting;
            this.Images = new(imageFileTargets);
            this.Trimming = new(setting.TrimmingTop, setting.TrimmingBottom, setting.TrimmingLeft, setting.TrimmingRight);
        }
    }
}

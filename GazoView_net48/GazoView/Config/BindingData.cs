using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Config
{
    class BindingData
    {
        public Setting Setting { get; set; }
        public State State { get; set; }
        public Theme Theme { get; set; }

        public BindingData()
        {
            this.Setting = Setting.Deserialize();
            this.State = new State();
            this.Theme = new Theme(new ThemeDefault());
        }
    }
}

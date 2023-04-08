using GazoView.Lib.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView
{
    class BindingParam
    {
        const string CONF_SETTING = "Setting.json";

        public Setting Setting { get; set; }

        /// <summary>
        /// アプリケーション起動時
        /// </summary>
        public BindingParam()
        {
            this.Setting = Setting.Load(CONF_SETTING);
        }

        /// <summary>
        /// アプリケーション終了時
        /// </summary>
        public void Close()
        {
            Setting.Save(CONF_SETTING);
        }
    }
}

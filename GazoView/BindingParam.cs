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
        public Setting Setting { get; set; }

        public State State { get; set; }

        public ImageStore Images { get; set; }

        public OpacityRate WindowOpacity { get; set; }

        public ScalingRate ImageSizeRate { get; set; }

        public bool MouseMovingInTrimming { get; set; }

        /// <summary>
        /// アプリケーション起動時
        /// </summary>
        public BindingParam()
        {
            this.Setting = Setting.Load(Item.FilePath.SettingFile);
            this.State = new();
            this.WindowOpacity = new();
            this.ImageSizeRate = new();
        }

        /// <summary>
        /// アプリケーション終了時
        /// </summary>
        public void Close()
        {
            Setting.Save(Item.FilePath.SettingFile);
        }
    }
}

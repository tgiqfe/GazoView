using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Windows;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace GazoView.Config
{
    /// <summary>
    /// 不揮発情報を格納
    /// </summary>
    public class Setting
    {
        private const string SETTING_FILENAME = "Setting.json";

        public double Width { get; set; }
        public double Height { get; set; }
        public double LocationX { get; set; }
        public double LocationY { get; set; }

        public string AlternateApp { get; set; }
        public int AutoFilelistUpdateInterval { get; set; }

        public Setting() { }

        public void Init()
        {
            this.Width = 640;
            this.Height = 480;
            this.LocationX = (SystemParameters.WorkArea.Width / 2) - (this.Width / 2);
            this.LocationY = (SystemParameters.WorkArea.Height / 2) - (this.Height / 2);
            this.AlternateApp = "mspaint.exe";
            this.AutoFilelistUpdateInterval = 1500;
        }

        /// <summary>
        /// 実行ファイルと同じフォルダーにある設定ファイルから読み込み
        /// </summary>
        /// <returns></returns>
        public static Setting Deserialize()
        {
            Setting setting = null;
            try
            {
                string settingPath = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SETTING_FILENAME);
                using (var sr = new StreamReader(settingPath, Encoding.UTF8))
                {
                    setting = JsonConvert.DeserializeObject<Setting>(sr.ReadToEnd());
                }
            }
            catch { }
            finally
            {
                if (setting == null)
                {
                    setting = new Setting();
                    setting.Init();
                }
            }
            return setting;
        }

        /// <summary>
        /// 実行ファイルと同じ場所に保存
        /// </summary>
        public void Serialize()
        {
            string settingPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SETTING_FILENAME);
            using(var sw = new StreamWriter(settingPath, false, Encoding.UTF8))
            {
                sw.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
            }
        }
    }
}

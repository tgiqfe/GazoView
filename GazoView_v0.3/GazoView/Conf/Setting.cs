using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GazoView.Conf
{
    internal class Setting
    {
        #region Binding and save parameter

        public double Width { get; set; }

        public double Height { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        #endregion

        #region save load

        const string _settingFileName = "setting.json";

        public static Setting Load()
        {
            string path = Path.Combine(
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                _settingFileName);

            try
            {
                using (var sr = new StreamReader(path, Encoding.UTF8))
                {
                    var setting = JsonSerializer.Deserialize<Setting>(sr.ReadToEnd());
                    return setting;
                }
            }
            catch
            {
                var setting = new Setting();
                setting.Init();
                return setting;
            }
        }

        public void Save()
        {
            string path = Path.Combine(
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                _settingFileName);

            using (var sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                });
                sw.Write(json);
            }
        }

        public void Init()
        {
            this.Width = 800;
            this.Height = 600;
            this.X = 50;
            this.Y = 50;
        }

        #endregion
    }
}

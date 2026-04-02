using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace GazoView
{
    public class Setting
    {
        const string SETTING_FILE = "Setting.json";

        /// <summary>
        /// Last ended windows width.
        /// </summary>
        public double Width { get; set; } = 800;

        /// <summary>
        /// Last ended window height.
        /// </summary>
        public double Height { get; set; } = 600;

        /// <summary>
        /// Last ended window left position.
        /// </summary>
        public double Left { get; set; } = 0;

        /// <summary>
        /// Last ended window top position.
        /// </summary>
        public double Top { get; set; } = 0;

        public int TrimmingTop { get; set; } = -1;
        public int TrimmingBottom { get; set; } = -1;
        public int TrimmingLeft { get; set; } = -1;
        public int TrimmingRight { get; set; } = -1;


        #region Load/Save setting

        /// <summary>
        /// Setting load from file. If failed, return default setting.
        /// </summary>
        /// <returns></returns>
        public static Setting Load()
        {
            var setting = new Setting();
            var path = Path.Combine(Item.WorkDirectory, SETTING_FILE);
            try
            {
                return JsonSerializer.Deserialize<Setting>(File.ReadAllText(path)) ?? setting;
            }
            catch { }
            return setting;
        }

        /// <summary>
        /// Setting save to file. If failed, do nothing.
        /// </summary>
        public void Save()
        {
            var path = Path.Combine(Item.WorkDirectory, SETTING_FILE);
            try
            {
                var text = JsonSerializer.Serialize(this, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                });
                File.WriteAllText(path, text);
            }
            catch { }
        }

        #endregion
    }
}

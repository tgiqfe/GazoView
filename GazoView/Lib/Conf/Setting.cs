using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GazoView.Lib.Conf
{
    internal class Setting
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }

        public int MaxTrimmingHistory { get; set; }
        public List<string> TrimmingHistries { get; set; }

        const string _fileName = "setting.json";

        public static Setting Load()
        {
            var path = Path.Combine(Item.WorkDirectory, _fileName);
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
                return new Setting()
                {
                    Width = 800,
                    Height = 600,
                    Left = 0,
                    Top = 0,
                    MaxTrimmingHistory = 10,
                    TrimmingHistries = new List<string>()
                };
            }
        }

        public void Save()
        {
            var path = Path.Combine(Item.WorkDirectory, _fileName);
            try
            {
                using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    var json = JsonSerializer.Serialize(this, new JsonSerializerOptions()
                    {
                        WriteIndented = true,
                    });
                    sw.Write(json);
                }
            }
            catch { }
        }
    }
}

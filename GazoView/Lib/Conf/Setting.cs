using System.IO;
using System.Text;
using System.Text.Json;

namespace GazoView.Lib.Conf
{
    class Setting
    {
        public string ApplicationVersion { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        const string FILE_NAME = "setting.json";
        const string APPLICATION_VERSION = "0.5.*";

        public Setting()
        {
            this.ApplicationVersion = APPLICATION_VERSION;
        }

        /// <summary>
        /// Load setting file.
        /// </summary>
        /// <returns></returns>
        public static Setting Load()
        {
            try
            {
                string json = File.ReadAllText(
                    Path.Combine(Item.WorkingDirectory, FILE_NAME), 
                    Encoding.UTF8);
                return JsonSerializer.Deserialize(json, typeof(Setting)) as Setting;
            }
            catch
            {
                return new Setting()
                {
                    Width = 800,
                    Height = 600,
                    X = 100,
                    Y = 100
                };
            }
        }

        /// <summary>
        /// Save setting file.
        /// </summary>
        public void Save()
        {
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
            File.WriteAllText(
                Path.Combine(Item.WorkingDirectory, FILE_NAME),
                json,
                Encoding.UTF8);
        }
    }
}

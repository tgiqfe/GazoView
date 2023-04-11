using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GazoView.Lib.Config
{
    public class Setting
    {
        #region Binding parameter

        /// <summary>
        /// ウィンドウサイズ(横)
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// ウィンドウサイズ(縦)
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// ウィンドウ位置(X軸)
        /// </summary>
        public double LocationX { get; set; }

        /// <summary>
        /// ウィンドウ位置(Y軸)
        /// </summary>
        public double LocationY { get; set; }

        #endregion
        #region Not binging parameter

        /// <summary>
        /// 外部アプリ1
        /// </summary>
        public string AltenateApp1 { get; set; }

        /// <summary>
        /// 外部アプリ2
        /// </summary>
        public string AltenateApp2 { get; set; }

        /// <summary>
        /// 外部アプリ3
        /// </summary>
        public string AltenateApp3 { get; set; }

        /// <summary>
        /// 外部アプリ4
        /// </summary>
        public string AltenateApp4 { get; set; }

        /// <summary>
        /// 外部アプリ5
        /// </summary>
        public string AltenateApp5 { get; set; }

        /// <summary>
        /// 外部アプリ6
        /// </summary>
        public string AltenateApp6 { get; set; }

        /// <summary>
        /// 外部アプリ7
        /// </summary>
        public string AltenateApp7 { get; set; }

        /// <summary>
        /// 外部アプリ8
        /// </summary>
        public string AltenateApp8 { get; set; }

        /// <summary>
        /// 外部アプリ9
        /// </summary>
        public string AltenateApp9 { get; set; }

        /// <summary>
        /// 外部アプリ10
        /// </summary>
        public string AltenateApp0 { get; set; }

        /// <summary>
        /// ファイルリストの自動アップデートインターバル
        /// </summary>
        public int FileListUpdateInterval { get; set; }

        #endregion
        #region save,load

        public static Setting Load(string path)
        {
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

        public void Save(string path)
        {
            try
            {
                using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    string json = JsonSerializer.Serialize(this, new JsonSerializerOptions()
                    {
                        WriteIndented = true,
                    });
                    sw.WriteLine(json);
                }
            }
            catch { }
        }

        public void Init()
        {
            this.Width = 800;
            this.Height = 560;
            this.LocationX = 50;
            this.LocationY = 50;
        }

        #endregion

    }
}

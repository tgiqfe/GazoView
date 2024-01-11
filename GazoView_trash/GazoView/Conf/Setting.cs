﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace GazoView.Conf
{
    internal class Setting
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
        public double X { get; set; }

        /// <summary>
        /// ウィンドウ位置(Y軸)
        /// </summary>
        public double Y { get; set; }

        #endregion

        /// <summary>
        /// ファイルリストの自動アップデートインターバル
        /// </summary>
        public int FileListUpdateInterval { get; set; }

        #region Save,Load

        private static string _settingFilePath = null;

        public static Setting Load()
        {
            _settingFilePath ??= Path.Combine(
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                "Setting.json");

            try
            {
                using (var sr = new StreamReader(_settingFilePath, Encoding.UTF8))
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
            _settingFilePath ??= Path.Combine(
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                "Setting.json");

            using (var sw = new StreamWriter(_settingFilePath, false, Encoding.UTF8))
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
            this.Height = 550;
            this.X = 50;
            this.Y = 50;
        }

        #endregion
    }
}
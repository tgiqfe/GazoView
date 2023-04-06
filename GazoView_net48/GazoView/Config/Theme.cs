using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GazoView.Config
{
    public class Theme
    {
        //private ThemeSetting Setting = null;

        private ThemeBase _theme = null;

        /// <summary>
        /// ウィンドウ全体の背景色
        /// </summary>
        private Brush _windowBackground = null; public Brush WindowBackground
        {
            get
            {
                if (_windowBackground == null)
                {
                    _windowBackground = GetBrush(_theme.WindowBackground, new SolidColorBrush(Color.FromArgb(0xFf, 0x40, 0x40, 0x40)));
                }
                return _windowBackground;
            }
        }

        /// <summary>
        /// ファイルをDragOver時の背景色
        /// </summary>
        private Brush _dragOverBackground = null; public Brush DoragOverBackground
        {
            get
            {
                if (_dragOverBackground == null)
                {
                    _dragOverBackground = GetBrush(_theme.DragOverBackground, Brushes.DodgerBlue);
                }
                return _dragOverBackground;
            }
        }

        /// <summary>
        /// タイトルバーのフォントの文字色
        /// </summary>
        private Brush _titleForeground = null;
        public Brush TitleForeground
        {
            get
            {
                if (_titleForeground == null)
                {
                    _titleForeground = GetBrush(_theme.TitleForeground, Brushes.Wheat);
                }
                return _titleForeground;
            }
        }

        /// <summary>
        /// ImageInfoパネルのフォントの文字色
        /// </summary>
        private Brush _imageInfoForeground = null;
        public Brush ImageInfoForeground
        {
            get
            {
                if (_imageInfoForeground == null)
                {
                    _imageInfoForeground = GetBrush(_theme.ImageInfoForeground, Brushes.White);
                }
                return _imageInfoForeground;
            }
        }

        public Theme() { }
        public Theme(ThemeBase theme)
        {
            this._theme = theme;
        }

        /// <summary>
        /// 色の名前からBrushを取得
        /// </summary>
        /// <param name="brushName"></param>
        /// <param name="defaultBrush"></param>
        /// <returns></returns>
        private Brush GetBrush(string brushName, Brush defaultBrush)
        {
            try
            {
                if (brushName.StartsWith("#"))
                {
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString(brushName));
                }
                else
                {
                    PropertyInfo pi = typeof(Brushes).
                        GetProperties(BindingFlags.Public | BindingFlags.Static).
                            FirstOrDefault(x => x.Name.Equals(brushName, StringComparison.OrdinalIgnoreCase));
                    return (Brush)pi.GetValue(typeof(Brushes));
                }
            }
            catch
            {
                return defaultBrush;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GazoView.Converter
{
    /// <summary>
    /// Bool値を文字列に変換するコンバーター
    /// </summary>
    internal class BoolToStringConverter : IValueConverter
    {
        /// <summary>
        /// Trueの場合に出力する文字列
        /// </summary>
        public string TrueValue { get; set; }

        /// <summary>
        /// Falseの場合に出力する文字列
        /// </summary>
        public string FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return string.IsNullOrEmpty(this.TrueValue) ? "True" : TrueValue;
            }
            else
            {
                return string.IsNullOrEmpty(this.FalseValue) ? "False" : FalseValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace GazoView.Converter
{
    /// <summary>
    /// 左側/上側のLineの座標を参照する為のコンバータ
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    class LessLinePositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value + 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return double.Parse((string)value) - 2;
        }
    }
}

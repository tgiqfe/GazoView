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
    /// 右側/下側のLineの座標を参照する為のコンバータ
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    class MoreLinePositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return double.Parse((string)value) + 2;
        }
    }
}

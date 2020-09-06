using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GazoView.Converter
{
    /// <summary>
    /// トリミングモードでトリミング結果のサイズを表示する為のコンバータ
    /// </summary>
    class TrimmingSizeResultConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double resultX = (double)values[1] - (double)values[0] - 4;
            double resultY = (double)values[3] - (double)values[2] - 4;

            if (resultX < 0) { resultX = 0; }
            if (resultY < 0) { resultY = 0; }

            return string.Format("{0} x {1}", resultX, resultY);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

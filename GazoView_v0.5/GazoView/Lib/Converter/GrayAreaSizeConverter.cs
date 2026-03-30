using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GazoView.Lib.Converter
{
    internal class GrayAreaSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter switch
            {
                "top" => values[0],
                "bottom" => (double)values[1] - (double)values[0],
                "left" => values[0],
                "right" => (double)values[1] - (double)values[0],
                _ => 0,
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

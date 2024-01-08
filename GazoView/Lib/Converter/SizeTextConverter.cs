using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GazoView.Lib.Converter
{
    internal class SizeTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var width = (double)values[0];
            var height = (double)values[1];
            return $"{width} x {height}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var size = (string)value;
            var sizeArray = size.Split('x');
            return new object[] { double.Parse(sizeArray[0]), double.Parse(sizeArray[1]) };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GazoView.Lib.Converter
{
    /// <summary>
    /// bool値とint値を受け取り、両方がtrue/1以上の場合にVisibility.Visibleを返す
    /// </summary>
    internal class MultiToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool val1 = (bool)values[0];   
            int val2 = (int)values[1];
            return val1 && val2 > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

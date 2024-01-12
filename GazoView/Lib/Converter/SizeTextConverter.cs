using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GazoView.Lib.Converter
{
    /// <summary>
    /// マルチBindingした、WidthとHeightを表示する。
    /// ConverterParameterに「Round」を指定すると、小数点第2位まで表示する。
    /// Converterparameterを指定しない場合は、そのまま表示。
    /// </summary>
    internal class SizeTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string param = parameter as string;
            return param switch
            {
                "Round" => $"{Math.Round((double)values[0], 2)} x {Math.Round((double)values[1], 2)}",
                _ => $"{(double)values[0]} x {(double)values[1]}"
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

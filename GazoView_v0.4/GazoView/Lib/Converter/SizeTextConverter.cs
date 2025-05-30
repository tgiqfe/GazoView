using System.Globalization;
using System.Windows.Data;

namespace GazoView.Lib.Converter
{
    internal class SizeTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string param = parameter as string;
            return param switch
            {
                "Round" => $"{Math.Round((double)values[0], 2)} x {Math.Round((double)values[1], 2)}",
                _ => $"{values[0]} x {values[1]}"
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

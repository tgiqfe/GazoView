using System.Globalization;
using System.Windows.Data;

namespace GazoView.Lib.Converter
{
    internal class TrimmingSizeToTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int left = (int)values[0];
            int top = (int)values[1];
            int right = (int)values[2];
            int bottom = (int)values[3];

            return $"{right - left} x {bottom - top}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

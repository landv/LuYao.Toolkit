using System;
using System.Globalization;
using System.Windows.Data;

namespace LuYao.Toolkit.Converters;

[ValueConversion(typeof(string), typeof(bool))]
public class StringIsNotNullOrEmptyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

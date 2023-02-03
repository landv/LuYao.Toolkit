using System;
using System.Globalization;
using System.Windows.Data;

namespace LuYao.Toolkit.Converters;

public class TruncateStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var max = 500;
        if (parameter is int i) max = i;
        if (max <= 0) max = 500;
        if (value is string str)
        {
            if (str.Length > max) return str.Substring(0, max);
            return str;
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

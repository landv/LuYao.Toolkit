using System;
using System.Globalization;
using System.Windows.Data;

namespace LuYao.Toolkit.Converters;

public class SingleLineConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str && !string.IsNullOrWhiteSpace(str))
        {
            return str
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Trim();
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

using System;
using System.Globalization;
using System.Windows.Data;

namespace LuYao.Toolkit.Converters;

public class DateTimeToRelativeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime time) return DateTimeUtils.GetRelative(time);
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

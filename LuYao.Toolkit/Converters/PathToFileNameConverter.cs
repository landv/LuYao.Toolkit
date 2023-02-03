using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace LuYao.Toolkit.Converters;

[ValueConversion(typeof(string), typeof(string))]
public class PathToFileNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str) return Path.GetFileName(str);
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

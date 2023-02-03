using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LuYao.Toolkit.Converters;

public class RegexMatchPositionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Match m)
        {
            return $"({m.Index},{m.Length})";
        }
        if (value is Group g)
        {
            return $"({g.Index},{g.Length})";
        }
        if (value is Capture c)
        {
            return $"({c.Index},{c.Length})";
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

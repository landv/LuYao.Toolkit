using LuYao.IO;
using NewLife.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LuYao.Toolkit.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    [ValueConversion(typeof(long), typeof(string))]
    public class ByteCountToDisplaySizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int || value is long || value is uint || value is ulong)
            {
                var len = System.Convert.ToInt64(value);
                return FileUtil.ByteCountToDisplaySize(len);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

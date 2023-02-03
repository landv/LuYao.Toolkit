using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LuYao.Toolkit.Converters
{
    public class LanguageCodeToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrWhiteSpace(str))
            {
                switch (str.ToUpperInvariant())
                {
                    case "EN": return "英语";
                    case "FR": return "法语";
                    case "DE": return "德语";
                    case "IT": return "意大利";
                    case "ES": return "西班牙";
                    case "PT": return "葡萄牙";
                    case "NL": return "荷兰语";
                    case "PL": return "波兰语";
                    case "JA": return "日语";
                    case "KO": return "韩语";
                    case "AR": return "阿拉伯";
                    case "TR": return "土耳其";
                    case "TH": return "泰文";
                    case "MS": return "马来语";
                    case "VI": return "越南语";
                    case "SV": return "瑞典语";
                    case "ID": return "印尼语";
                    case "ZH":
                    case "ZH-CN":
                        return "中文";
                    default: return str;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using LuYao.Toolkit.Channels;
using LuYao.Toolkit.Tabs.Navs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace LuYao.Toolkit.Converters
{
    public class FunctionBackgroundConverter : IValueConverter
    {
        private static readonly IReadOnlyList<SolidColorBrush> BrusheList = new SolidColorBrush[]
        {
            new SolidColorBrush(Color.FromArgb(255,0,0,255)),
            new SolidColorBrush(Color.FromArgb(255,0,98,102)),
            new SolidColorBrush(Color.FromArgb(255,0,123,67)),
            new SolidColorBrush(Color.FromArgb(255,0,148,50)),
            new SolidColorBrush(Color.FromArgb(255,0,148,200)),
            new SolidColorBrush(Color.FromArgb(255,0,149,217)),
            new SolidColorBrush(Color.FromArgb(255,0,163,175)),
            new SolidColorBrush(Color.FromArgb(255,0,164,151)),
            new SolidColorBrush(Color.FromArgb(255,0,168,255)),
            new SolidColorBrush(Color.FromArgb(255,0,181,204)),
            new SolidColorBrush(Color.FromArgb(255,0,184,148)),
            new SolidColorBrush(Color.FromArgb(255,0,206,201)),
            new SolidColorBrush(Color.FromArgb(255,1,152,117)),
            new SolidColorBrush(Color.FromArgb(255,1,163,164)),
            new SolidColorBrush(Color.FromArgb(255,3,201,169)),
            new SolidColorBrush(Color.FromArgb(255,6,82,221)),
            new SolidColorBrush(Color.FromArgb(255,9,132,227)),
            new SolidColorBrush(Color.FromArgb(255,10,189,227)),
            new SolidColorBrush(Color.FromArgb(255,16,172,132)),
            new SolidColorBrush(Color.FromArgb(255,18,137,167)),
            new SolidColorBrush(Color.FromArgb(255,34,166,179)),
            new SolidColorBrush(Color.FromArgb(255,39,60,117)),
            new SolidColorBrush(Color.FromArgb(255,39,146,195)),
            new SolidColorBrush(Color.FromArgb(255,44,169,225)),
            new SolidColorBrush(Color.FromArgb(255,46,134,222)),
            new SolidColorBrush(Color.FromArgb(255,46,213,115)),
            new SolidColorBrush(Color.FromArgb(255,52,31,151)),
            new SolidColorBrush(Color.FromArgb(255,52,158,105)),
            new SolidColorBrush(Color.FromArgb(255,56,161,219)),
            new SolidColorBrush(Color.FromArgb(255,56,180,139)),
            new SolidColorBrush(Color.FromArgb(255,71,88,92)),
            new SolidColorBrush(Color.FromArgb(255,72,52,212)),
            new SolidColorBrush(Color.FromArgb(255,72,126,176)),
            new SolidColorBrush(Color.FromArgb(255,76,209,55)),
            new SolidColorBrush(Color.FromArgb(255,77,5,232)),
            new SolidColorBrush(Color.FromArgb(255,78,205,196)),
            new SolidColorBrush(Color.FromArgb(255,89,88,87)),
            new SolidColorBrush(Color.FromArgb(255,106,176,76)),
            new SolidColorBrush(Color.FromArgb(255,108,92,231)),
            new SolidColorBrush(Color.FromArgb(255,111,30,81)),
            new SolidColorBrush(Color.FromArgb(255,113,128,142)),
            new SolidColorBrush(Color.FromArgb(255,127,143,166)),
            new SolidColorBrush(Color.FromArgb(255,131,52,113)),
            new SolidColorBrush(Color.FromArgb(255,131,149,167)),
            new SolidColorBrush(Color.FromArgb(255,145,61,136)),
            new SolidColorBrush(Color.FromArgb(255,153,128,250)),
            new SolidColorBrush(Color.FromArgb(255,156,136,255)),
            new SolidColorBrush(Color.FromArgb(255,163,203,56)),
            new SolidColorBrush(Color.FromArgb(255,165,55,253)),
            new SolidColorBrush(Color.FromArgb(255,181,52,113)),
            new SolidColorBrush(Color.FromArgb(255,190,46,221)),
            new SolidColorBrush(Color.FromArgb(255,211,56,28)),
            new SolidColorBrush(Color.FromArgb(255,214,48,49)),
            new SolidColorBrush(Color.FromArgb(255,217,51,63)),
            new SolidColorBrush(Color.FromArgb(255,217,128,250)),
            new SolidColorBrush(Color.FromArgb(255,225,112,85)),
            new SolidColorBrush(Color.FromArgb(255,225,123,52)),
            new SolidColorBrush(Color.FromArgb(255,230,0,51)),
            new SolidColorBrush(Color.FromArgb(255,232,65,24)),
            new SolidColorBrush(Color.FromArgb(255,232,67,147)),
            new SolidColorBrush(Color.FromArgb(255,233,82,149)),
            new SolidColorBrush(Color.FromArgb(255,234,32,39)),
            new SolidColorBrush(Color.FromArgb(255,234,85,6)),
            new SolidColorBrush(Color.FromArgb(255,235,77,75)),
            new SolidColorBrush(Color.FromArgb(255,235,98,56)),
            new SolidColorBrush(Color.FromArgb(255,237,76,103)),
            new SolidColorBrush(Color.FromArgb(255,238,82,83)),
            new SolidColorBrush(Color.FromArgb(255,238,90,36)),
            new SolidColorBrush(Color.FromArgb(255,240,147,43)),
            new SolidColorBrush(Color.FromArgb(255,243,104,224)),
            new SolidColorBrush(Color.FromArgb(255,245,36,67)),
            new SolidColorBrush(Color.FromArgb(255,245,171,53)),
            new SolidColorBrush(Color.FromArgb(255,246,36,89)),
            new SolidColorBrush(Color.FromArgb(255,247,159,31)),
            new SolidColorBrush(Color.FromArgb(255,247,202,24)),
            new SolidColorBrush(Color.FromArgb(255,251,197,49)),
            new SolidColorBrush(Color.FromArgb(255,253,121,168)),
            new SolidColorBrush(Color.FromArgb(255,253,203,110)),
            new SolidColorBrush(Color.FromArgb(255,255,71,209)),
            new SolidColorBrush(Color.FromArgb(255,255,107,129)),
            new SolidColorBrush(Color.FromArgb(255,255,159,67)),
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FunctionItem item)
            {
                var code = Math.Abs(item.Id.GetHashCode());
                var len = BrusheList.Count;
                var idx = code % len;
                return BrusheList[idx];
            }
            if(value is NavItem nav)
            {
                var code = Math.Abs(nav.Url.GetDeterministicHashCode());
                var len = BrusheList.Count;
                var idx = code % len;
                return BrusheList[idx];
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

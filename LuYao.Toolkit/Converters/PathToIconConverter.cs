using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.IO;

namespace LuYao.Toolkit.Converters;

[ValueConversion(typeof(string), typeof(ImageSource))]
public class PathToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string fn && File.Exists(fn))
        {
            //return Path.GetFileName(str);
            var icon = Icon.ExtractAssociatedIcon(fn);
            if (icon != null)
            {
                using (var bitmap = icon.ToBitmap())
                {
                    //ensure Bitmap is disposed of after usefulness is fulfilled.
                    var bmpPt = bitmap.GetHbitmap();
                    try
                    {
                        var bitmapSource =
                            Imaging.CreateBitmapSourceFromHBitmap(
                                bmpPt,
                                IntPtr.Zero,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());

                        //freeze bitmapSource and clear memory to avoid memory leaks
                        bitmapSource.Freeze();
                        return bitmapSource;
                    }
                    finally
                    {
                        //done in a finally block to ensure this memory is not leaked regardless of exceptions.
                        DeleteObject(bmpPt);
                    }
                }
            }
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    [DllImport("gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool DeleteObject(IntPtr value);
}

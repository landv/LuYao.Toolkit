using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LuYao.Toolkit.Controls
{
    public class TextBlockWithIconBox : Control
    {
        static TextBlockWithIconBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBlockWithIconBox), new FrameworkPropertyMetadata(typeof(TextBlockWithIconBox)));
        }


        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(TextBlockWithIconBox), new PropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextBlockWithIconBox), new PropertyMetadata(string.Empty));

    }
}

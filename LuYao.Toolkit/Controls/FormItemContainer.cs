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

namespace LuYao.Toolkit.Controls;

public class FormItemContainer : ContentControl
{
    static FormItemContainer()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FormItemContainer), new FrameworkPropertyMetadata(typeof(FormItemContainer)));
    }
    public static readonly DependencyProperty PrefixProperty =
        DependencyProperty.Register("Prefix", typeof(string), typeof(FormItemContainer)
            , new PropertyMetadata("标签信息："));

    public static readonly DependencyProperty PrefixWidthProperty =
        DependencyProperty.Register("PrefixWidth", typeof(double), typeof(FormItemContainer),
            new PropertyMetadata(100.0d));
    public string Prefix
    {
        get => (string)GetValue(PrefixProperty);
        set => SetValue(PrefixProperty, value);
    }


    public double PrefixWidth
    {
        get => (double)GetValue(PrefixWidthProperty);
        set => SetValue(PrefixWidthProperty, value);
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (Content is UIElement element)
        {
            element.Focus();
        }

        base.OnMouseDown(e);
    }
}

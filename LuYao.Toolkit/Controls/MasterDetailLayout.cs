using System;
using System.Collections;
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

public class MasterDetailLayout : Expander
{
    static MasterDetailLayout()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MasterDetailLayout), new FrameworkPropertyMetadata(typeof(MasterDetailLayout)));
    }
    public MasterDetailLayout()
    {
        this.CommandBindings.Add(new CommandBinding(LuYaoCommands.CloseDetailCommand, this.HandleCloseDetailCommand));
    }

    private void HandleCloseDetailCommand(object sender, ExecutedRoutedEventArgs e)
    {
        this.IsExpanded = false;
    }

    public double MinDetailWidth
    {
        get { return (double)GetValue(MinDetailWidthProperty); }
        set { SetValue(MinDetailWidthProperty, value); }
    }
    public double MaxDetailWidth
    {
        get { return (double)GetValue(MaxDetailWidthProperty); }
        set { SetValue(MaxDetailWidthProperty, value); }
    }

    public static readonly DependencyProperty MinDetailWidthProperty = DependencyProperty.Register("MinDetailWidth", typeof(double), typeof(MasterDetailLayout), new FrameworkPropertyMetadata(350d));
    public static readonly DependencyProperty MaxDetailWidthProperty = DependencyProperty.Register("MaxDetailWidth", typeof(double), typeof(MasterDetailLayout), new FrameworkPropertyMetadata(400d));
}

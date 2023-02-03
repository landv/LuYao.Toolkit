using HandyControl.Tools;
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

namespace LuYao.Toolkit.Update;

/// <summary>
/// FindNewVersionDialog.xaml 的交互逻辑
/// </summary>
public partial class FindNewVersionDialog : UserControl
{
    public FindNewVersionDialog()
    {
        InitializeComponent();
        this.Loaded += FindNewVersionDialog_Loaded;
    }

    private void FindNewVersionDialog_Loaded(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this).Topmost = true;
    }
}

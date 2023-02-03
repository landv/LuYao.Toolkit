using LuYao.IO.Updating;
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
/// Update.xaml 的交互逻辑
/// </summary>
public partial class Update : UserControl
{
    public Update(UpdatePackage package)
    {
        InitializeComponent();
        this.DataContext = new UpdateViewModel(package);
    }
}

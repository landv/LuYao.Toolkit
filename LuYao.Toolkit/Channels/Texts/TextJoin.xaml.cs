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

namespace LuYao.Toolkit.Channels.Texts;

/// <summary>
/// TextJoin.xaml 的交互逻辑
/// </summary>
[Views.ViewName(Views.ViewNames.Channels.Texts.TextJoin)]
public partial class TextJoin : UserControl
{
    public TextJoin()
    {
        InitializeComponent();
        Attaches.ComboBoxAttach.SetBindEnum(this.SplitComboBox, typeof(TextJoinViewModel.SplitBy));
        Attaches.ComboBoxAttach.SetBindEnum(this.EscapeComboBox, typeof(TextJoinViewModel.EscapeType));
        Attaches.ComboBoxAttach.SetBindEnum(this.JoinComboBox, typeof(TextJoinViewModel.JoinBy));
    }
}

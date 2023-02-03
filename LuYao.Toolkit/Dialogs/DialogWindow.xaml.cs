using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace LuYao.Toolkit.Dialogs;

/// <summary>
/// DialogWindow.xaml 的交互逻辑
/// </summary>
public partial class DialogWindow : IDialogWindow
{
    public DialogWindow()
    {
        InitializeComponent();
        this.Loaded += DialogWindow_Loaded;
    }

    private void DialogWindow_Loaded(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine(this.Title);
    }

    /// <summary>
    /// The <see cref="IDialogResult"/> of the dialog.
    /// </summary>
    public IDialogResult Result { get; set; }

}

using LuYao.Toolkit.Services;
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

namespace LuYao.Toolkit.Dialogs
{
    /// <summary>
    /// MultiboxingDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MultiboxingDialog : UserControl, IGrowlTokenProvider
    {
        public MultiboxingDialog()
        {
            InitializeComponent();
            this.GrowlToken = $"growl_{Math.Abs(this.GetHashCode())}";
            this.GrowlStackPanel.SetValue(HandyControl.Controls.Growl.TokenProperty, this.GrowlToken);
        }

        public string GrowlToken { get; set; }
    }
}

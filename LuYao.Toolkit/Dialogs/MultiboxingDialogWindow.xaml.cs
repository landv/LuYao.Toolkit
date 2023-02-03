using LuYao.Toolkit.Services;
using Prism.Services.Dialogs;
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
using System.Windows.Shapes;

namespace LuYao.Toolkit.Dialogs
{
    /// <summary>
    /// DialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MultiboxingDialogWindow : IDialogWindow, IGrowlTokenProvider
    {
        public MultiboxingDialogWindow()
        {
            InitializeComponent();
            this.Loaded += DialogWindow_Loaded;
        }

        private void DialogWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Owner = null;
        }

        public IDialogResult Result { get; set; }

        public string GrowlToken => GetGrowlName();
        private string GetGrowlName()
        {
            if (this.Content is IGrowlTokenProvider provider) return provider.GrowlToken;
            return nameof(MainWindow);
        }
    }
}

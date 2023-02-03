using LuYao.Toolkit.Rdm;
using LuYao.Toolkit.Rdm.Events;
using LuYao.Toolkit.Tabs.Rdp.Controls;
using LuYao.Toolkit.Views;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Windows.Controls;

namespace LuYao.Toolkit.Tabs.Rdp
{
    /// <summary>
    /// Index.xaml 的交互逻辑
    /// </summary>
    [ViewName(ViewNames.Tabs.Rdp.Index)]
    public partial class Index : UserControl
    {
        public Index()
        {
            InitializeComponent();
            OpenRdpConnection += Index_OpenRdpConnection;
        }

        private void Index_OpenRdpConnection(object sender, Guid id)
        {
            foreach (RdpTabItem item in SessionTabControl.Items)
            {
                if (item.Id == id)
                {
                    this.SessionTabControl.SelectedItem = item;
                    return;
                }
            }
            var e = Entities.RdpConnection.FindById(id);
            if (e == null) return;
            var rdp = new RdpConnection(e);
            var tab = new RdpTabItem(id, rdp);
            this.SessionTabControl.Items.Add(tab);
            this.SessionTabControl.SelectedItem = tab;
            tab.Connect();
        }

        private static event EventHandler<Guid> OpenRdpConnection;
        public static void Open(Guid id)
        {
            if (OpenRdpConnection == null) return;
            OpenRdpConnection.Invoke(null, id);
        }
    }
}

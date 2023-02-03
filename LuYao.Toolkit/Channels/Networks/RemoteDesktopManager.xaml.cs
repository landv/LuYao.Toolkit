using LuYao.Toolkit.Rdm.Dialogs;
using LuYao.Toolkit.Rdm.Events;
using Prism.Events;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LuYao.Toolkit.Channels.Networks
{
    /// <summary>
    /// RemoteDesktopManager.xaml 的交互逻辑
    /// </summary>
    [Views.ViewName(Views.ViewNames.Channels.Networks.RemoteDesktopManager)]
    public partial class RemoteDesktopManager : UserControl
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogService dialogService;
        public RemoteDesktopManager(IEventAggregator eventAggregator, IDialogService dialogService)
        {
            InitializeComponent();
            this.eventAggregator = eventAggregator ?? throw new System.ArgumentNullException(nameof(eventAggregator));
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            this.eventAggregator.GetEvent<AddRdpConnectionEvent>().Subscribe(this.OnAddRdoConnection);
            this.eventAggregator.GetEvent<EditRdpConnectionEvent>().Subscribe(this.OnEditRdoConnection);
            this.eventAggregator.GetEvent<OpenRdpConnectionEvent>().Subscribe(this.OnOpenRdpConnection);
            this.eventAggregator.GetEvent<OpenRdmSettingEvent>().Subscribe(this.OnOpenRdmSetting);
        }

        private void OnOpenRdmSetting()
        {
            dialogService.ShowDialog(nameof(RdmSettingDialog), null, (r) => { this.eventAggregator.GetEvent<AfterRdmSettingEvent>().Publish(); });
        }

        private void OnOpenRdpConnection(OpenRdpConnectionEventPayload args)
        {
            //foreach (RdpTabItem item in SessionTabControl.Items)
            //{
            //    if (item.Id == args.Id)
            //    {
            //        this.SessionTabControl.SelectedItem = item;
            //        return;
            //    }
            //}
            //var e = Entities.RdpConnection.FindById(args.Id);
            //if (e == null) return;
            //var rdp = new RdpConnection(e);
            //var tab = new RdpTabItem(args.Id, rdp);
            //this.SessionTabControl.Items.Add(tab);
            //this.SessionTabControl.SelectedItem = tab;
            //tab.Connect();
        }

        private void DialogCallback(IDialogResult i)
        {
            if (i.Result == ButtonResult.OK)
            {
                this.eventAggregator.GetEvent<RdpConnectionChangedEvent>().Publish();
            }
        }

        private void OnEditRdoConnection(EditRdpConnectionEventPayload args)
        {
            var p = new DialogParameters();
            p.Add("act", "edit");
            p.Add("id", args.Id);
            dialogService.ShowDialog(nameof(RdpConnectionDetailDialog), p, DialogCallback);
        }

        private void OnAddRdoConnection()
        {
            var p = new DialogParameters();
            p.Add("act", "add");
            dialogService.ShowDialog(nameof(RdpConnectionDetailDialog), p, DialogCallback);
        }
    }
}

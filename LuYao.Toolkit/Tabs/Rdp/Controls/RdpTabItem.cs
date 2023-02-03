using LuYao.Toolkit.Rdm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Media;

namespace LuYao.Toolkit.Tabs.Rdp.Controls;

public class RdpTabItem : HandyControl.Controls.TabItem
{
    public RdpTabItem(Guid id, IRdpConnection connection)
    {
        this.Id = id;
        this.Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _session = new RdpSession() { RdpConnection = connection };
        this.Header = new RdpTabItemHeader(connection, _session);
        this.Content = new WindowsFormsHost
        {
            Child = _session,
            Margin = new Thickness(0),
            Padding = new Thickness(0),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };
        this.Closed += RdpTabItem_Closed;
    }

    private void RdpTabItem_Closed(object sender, EventArgs e)
    {
        if (_session == null) return;
        this.Dispatcher.BeginInvoke(() =>
        {
            this._session.Release();
        });
    }

    private RdpSession _session;
    public Guid Id { get; }
    public IRdpConnection Connection { get; }
    public void Connect()
    {
        this._session.Connect();
    }
    public void Disconnect()
    {
        this._session.Disconnect();
    }
}

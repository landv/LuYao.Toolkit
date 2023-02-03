using AxMSTSCLib;
using LuYao.Toolkit.Rdm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuYao.Toolkit.Tabs.Rdp.Controls;
public partial class RdpSession : UserControl, IMsRdpClientHandler
{
    static RdpSession() => LoadVersion();
    #region 版本识别
    public static int? Version { get; private set; }
    private static bool LoadVersion()
    {
        try
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\mstscax.dll");
            switch (versionInfo.ProductMajorPart)
            {
                default:
                    return false;
                case 10:
                    Version = 8;
                    break;
                case 6:
                    switch (versionInfo.ProductMinorPart)
                    {
                        default:
                            Version = 8;
                            break;
                        case 0:
                            if (versionInfo.ProductBuildPart <= 6000)
                            {
                                Version = 5;
                            }
                            else if (versionInfo.ProductBuildPart >= 6001)
                            {
                                Version = 6;
                            }
                            break;
                        case 1:
                            if (versionInfo.ProductBuildPart <= 6999)
                            {
                                Version = 6;
                            }
                            else if (versionInfo.ProductBuildPart >= 7000)
                            {
                                Version = 7;
                            }
                            break;
                        case 2:
                            if (versionInfo.ProductBuildPart >= 9200)
                            {
                                Version = 8;
                            }
                            break;
                        case 3:
                            if (versionInfo.ProductBuildPart >= 9600)
                            {
                                Version = 8;
                            }
                            break;
                    }
                    break;
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion
    private RdpConnectStatus _status = RdpConnectStatus.Pending;
    private static readonly object EventStatusChanged = new object();
    public RdpConnectStatus Status
    {
        get => _status;
        set
        {
            if (value != _status)
            {
                _status = value;
                var handlers = (StatusChangedEventHandler)Events[EventStatusChanged];
                handlers?.Invoke(this, new StatusChangedEventArgs(value));
            }
        }
    }
    public event StatusChangedEventHandler StatusChanged
    {
        add => base.Events.AddHandler(EventStatusChanged, value);
        remove => base.Events.RemoveHandler(EventStatusChanged, value);
    }
    public IMsRdpClient MsRdpClient { get; private set; }
    public IRdpConnection RdpConnection { get; set; }
    public RdpSession()
    {
        InitializeComponent();
        this.MsRdpClient = MsRdpClientFactory.Create(Version);
        this.Load += RdoSession_Load;
        this.tConnect.Tick += TConnect_Tick;
    }


    public void Release()
    {
        if (this.MsRdpClient == null) return;
        this.MsRdpClient.Detach(this);
        this.Controls.Remove(this.MsRdpClient.Control);
        this.MsRdpClient.Dispose();
        this.MsRdpClient = null;
    }

    public void Connect()
    {
        this.tConnect.Enabled = true;
    }
    public void Disconnect()
    {
        if (this.MsRdpClient == null) return;
        MsRdpClient.Disconnect();
    }
    private void TConnect_Tick(object sender, EventArgs e)
    {
        this.tConnect.Enabled = false;
        if (this.MsRdpClient == null || this.RdpConnection == null) return;
        MsRdpClient.Update(this.RdpConnection);
        MsRdpClient.Connect();
    }

    private void RdoSession_Load(object sender, EventArgs e)
    {
        if (this.MsRdpClient != null)
        {
            var ctrl = MsRdpClient.Control;
            ctrl.Enabled = true;
            ctrl.Dock = DockStyle.Fill;
            this.Controls.Add(ctrl);
            MsRdpClient.Attach(this);
        }
        else
        {
            Status = RdpConnectStatus.Fail;
            MessageBox.Show(
                "Unable to find compatible version of Microsoft Remote Desktop Connection!" + Environment.NewLine + "Version 6.0 or higher is required.",
                $"{this.Name} - Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Hand
            );
        }
    }
    #region IMsRdpClientHandler
    public void OnConnecting(object sender, EventArgs e)
    {
        ErrorTextBox.SendToBack();
        ErrorTextBox.Visible = false;
        this.Status = RdpConnectStatus.Connecting;
    }

    public void OnLoginComplete(object sender, EventArgs e)
    {
    }

    public void OnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
    {
        if (e.discReason != 1 && e.discReason != 2 && MsRdpClient != null)
        {
            ErrorTextBox.Text = MsRdpClient.GetErrorDescription((uint)e.discReason, (uint)MsRdpClient.ExtendedDisconnectReason);
            ErrorTextBox.Visible = true;
            ErrorTextBox.BringToFront();
        }
        this.Status = RdpConnectStatus.Disconnected;
    }

    public void OnConnected(object sender, EventArgs e)
    {
        this.Status = RdpConnectStatus.Connected;
    }
    #endregion
}

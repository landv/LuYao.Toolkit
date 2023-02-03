using MSTSCLib;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace LuYao.Toolkit.Rdm;

public interface IMsRdpClient : IDisposable, ISupportInitialize
{
    void Update(IRdpConnection connection);
    Control Control { get; }
    void Attach(IMsRdpClientHandler handler);
    void Detach(IMsRdpClientHandler handler);
    void Connect();
    void Disconnect();
    string GetErrorDescription(uint disconnectReason, uint extendedDisconnectReason);
    ExtendedDisconnectReasonCode ExtendedDisconnectReason { get; }
}

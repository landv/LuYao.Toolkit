using AxMSTSCLib;
using System;

namespace LuYao.Toolkit.Rdm;

public interface IMsRdpClientHandler
{
    void OnConnecting(object sender, EventArgs e);

    void OnLoginComplete(object sender, EventArgs e);

    void OnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e);

    void OnConnected(object sender, EventArgs e);
}

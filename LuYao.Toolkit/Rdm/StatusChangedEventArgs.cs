using System;

namespace LuYao.Toolkit.Rdm;

public class StatusChangedEventArgs : EventArgs
{
    public RdpConnectStatus Status { get; }

    public StatusChangedEventArgs(RdpConnectStatus status) => Status = status;
}

public delegate void StatusChangedEventHandler(object sender, StatusChangedEventArgs e);
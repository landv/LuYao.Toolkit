using System;

namespace LuYao.Toolkit.Rdm.Events;

public class OpenRdpConnectionEventPayload
{
    public OpenRdpConnectionEventPayload(Guid id) => this.Id = id;
    public Guid Id { get; }
}

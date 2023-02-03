using System;
using System.Collections.Generic;
using System.Text;

namespace LuYao.Toolkit.Rdm.Events;

public class EditRdpConnectionEventPayload
{
	public EditRdpConnectionEventPayload(Guid id)
	{
		this.Id = id;
	}
    public Guid Id { get; }
}

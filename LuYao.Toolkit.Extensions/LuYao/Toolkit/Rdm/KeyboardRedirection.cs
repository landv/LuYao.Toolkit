using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Rdm;

public enum KeyboardRedirection
{
	[Description("在这台计算机上")]
	OnLocal,
	[Description("在远程计算机上")]
	OnRemote
}

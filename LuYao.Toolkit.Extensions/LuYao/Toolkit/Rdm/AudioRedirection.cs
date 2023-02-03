using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Rdm;

public enum AudioRedirection
{
	[Description("在此计算机上播放")]
	PlayOnClient,
	[Description("在远程计算机上播放")]
	PlayOnRemote,
	[Description("不要播放")]
	PlayNoSound
}

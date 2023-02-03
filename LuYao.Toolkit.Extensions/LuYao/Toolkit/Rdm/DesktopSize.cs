using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Rdm;

public enum DesktopSize
{
	[Description("640 x 480")]
	X640Y480,
	[Description("800 x 600")]
	X800Y600,
	[Description("1024 x 768")]
	X1024Y768,
	[Description("1280 x 1024")]
	X1280Y1024,
	[Description("自定义")]
	Custom
}

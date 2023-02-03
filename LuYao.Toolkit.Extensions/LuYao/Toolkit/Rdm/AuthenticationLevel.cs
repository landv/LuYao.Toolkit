using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Rdm;

public enum AuthenticationLevel
{
    [Description("链接并且不显示警告")]
    Connect,
    [Description("不链接")]
    DoNotConnect,
    [Description("显示警告")]
    WarnMe
}
